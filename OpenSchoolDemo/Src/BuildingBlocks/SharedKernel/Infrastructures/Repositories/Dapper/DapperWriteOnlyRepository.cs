using Caching;
using MassTransit.Internals;
using Microsoft.Extensions.Localization;
using MySqlConnector;
using SharedKernel.Auth;
using SharedKernel.Contracts;
using SharedKernel.Contracts.Repositories;
using SharedKernel.Domain;
using SharedKernel.Libraries;
using SharedKernel.MySQL;
using SharedKernel.Properties;
using SharedKernel.Runtime.Exceptions;
using SharedKernel.UnitOfWork;

namespace SharedKernel.Infrastructures;

public class DapperWriteOnlyRepository<TEntity, TKey> : IDapperWriteOnlyRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
{
    protected IDbConnection _dbConnection;
    protected readonly string _tableName;
    protected readonly ICurrentUser _currentUser;
    protected readonly ISequenceCaching _sequenceCaching;
    protected readonly bool _isSystemTable;
    protected readonly IStringLocalizer<Resources> _localizer;

    public DapperWriteOnlyRepository(
        IDbConnection dbConnection,
        ICurrentUser currentUser,
        ISequenceCaching sequenceCaching,
        IStringLocalizer<Resources> localizer)
    {
        _dbConnection = dbConnection;
        _currentUser = currentUser;
        _sequenceCaching = sequenceCaching;
        _localizer = localizer;
        _tableName = ((TEntity)Activator.CreateInstance(typeof(TEntity))).GetTableName();
        _isSystemTable = typeof(TEntity).GetProperty("OwnerId") == null;
    }

    public IUnitOfWork UnitOfWork => _dbConnection;
    
    public async Task<TEntity> SaveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return (await SaveAsync(new List<TEntity> { entity }, cancellationToken)).First();
    }

    public virtual async Task<List<TEntity>> SaveAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
    {
        BeforeSave(entities);
        await BulkInsertAsync(entities, cancellationToken);

        return entities;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, IList<string> updateFields = default, CancellationToken cancellationToken = default)
    {
        var isSoftDelete = entity is ISoftDelete;
        var queryCmd = isSoftDelete ? $"SELECT * FROM {_tableName} WHERE Id = @Id AND IsDeleted = 0" : $"SELECT * FROM {_tableName} WHERE Id = @Id";
        var currentEntity = await _dbConnection.QuerySingleOrDefaultAsync<TEntity>(queryCmd, new { entity.Id })
                            ?? throw new BadRequestException(_localizer["repository_data_does_not_exist_or_was_deleted"].Value);

        var sqlCommand = $"UPDATE {_tableName} AS T SET ";
        var columnParams = new List<string>();
        var ignoreFields = new string[]
        {
            nameof(IDomainEntity.DomainEvents),
            nameof(EntityBase<TKey>.Id),
            nameof(EntityAuditBase<TKey>.CreatedDate),
            nameof(EntityAuditBase<TKey>.CreatedBy),
            nameof(EntityAuditBase<TKey>.IsDeleted),
            nameof(IPersonalizeEntity.OwnerId),
        };
        var properties = entity.GetPropertyInfos();
        
        if (updateFields != null)
        {
            if(typeof(TEntity).HasInterface<IDateTracking>())
            {
                updateFields.Add("LastModifiedDate");
                updateFields.Add("LastModifiedBy");
            }
            updateFields = updateFields.Distinct().ToList();
        }
        
        foreach (var prop in properties)
        {
            if (updateFields != null && updateFields.Any() && !updateFields.Contains(prop.Name))
            {
                entity[prop.Name] = currentEntity[prop.Name];
            }
            if (!ignoreFields.Contains(prop.Name))
            {
                columnParams.Add($"T.{prop.Name} = @{prop.Name}");
            }
        }
        
        BeforeUpdate(entity);
        
        sqlCommand += string.Join(", ", columnParams) + " WHERE T.Id = @Id";
        if (entity is IPersonalizeEntity)
        {
            sqlCommand += $" AND IF(OwnerId = '{_currentUser.Context.OwnerId}', TRUE, FALSE)";
        }

        if (entity is IUserTracking)
        {
            sqlCommand += $" AND IF(CreatedBy = '{_currentUser.Context.OwnerId}', TRUE, FALSE)";
        }
        
        if(isSoftDelete)
        {
            sqlCommand += $" AND T.IsDeleted = 0";
        }

        await _dbConnection.ExecuteAsync(sqlCommand, entity, cancellationToken: cancellationToken);

        await ClearCacheWhenChangesAsync(new List<TKey> { entity.Id }, cancellationToken);
        return currentEntity;
        
    }

    public async Task<List<TEntity>> DeleteAsync(List<TKey> ids, CancellationToken cancellationToken = default)
    {
        var joinedIds = string.Join(", ", ids);
        if (Secure.DetectSqlInjection(joinedIds))
        {
            throw new SqlInjectionException();
        }

        var sqlCommand = $"SELECT * FROM {_tableName} AS T WHERE T.Id In ( {joinedIds}) AND T.IsDeleted = 0";
        var deleteCommand = "";
        
        if (typeof(TEntity).HasInterface<ISoftDelete>())
        {
            deleteCommand = $"UPDATE {_tableName} AS T SET T.IsDeleted = 1, T.DeletedDate = @DeletedDate, T.DeletedBy = @DeletedBy WHERE T.Id IN( {joinedIds} )";
        }
        else
        {
            deleteCommand = $"DELETE FROM {_tableName} WHERE  T.Id IN( {joinedIds} )";
        }
        
        if (typeof(TEntity).HasInterface<IPersonalizeEntity>())
        {
            sqlCommand += $" AND IF(OwnerId = '{_currentUser.Context.OwnerId}', TRUE, FALSE)";
            deleteCommand += $" AND IF(OwnerId = '{_currentUser.Context.OwnerId}', TRUE, FALSE))";
        }
        
        if(typeof(TEntity).HasInterface<IUserTracking>())
        {
            sqlCommand += $" AND IF(CreatedBy = '{_currentUser.Context.OwnerId}', TRUE, FALSE);";
            deleteCommand += $" AND IF(CreatedBy = '{_currentUser.Context.OwnerId}', TRUE, FALSE);";
        }
        
        var entities = await _dbConnection.QueryAsync<TEntity>(sqlCommand);
        
        if (entities.Any())
        {
            BeforeDelete(entities);
            
            if (typeof(TEntity).HasInterface<ISoftDelete>())
            {
                var param = new
                {
                    DeletedDate = DateHelper.Now,
                    DeletedBy = _currentUser.Context.OwnerId
                };
                await _dbConnection.ExecuteAsync(deleteCommand, param, cancellationToken: cancellationToken);
            }
            else
            {
                await _dbConnection.ExecuteAsync(deleteCommand, null, cancellationToken: cancellationToken);
            }
        }
        await ClearCacheWhenChangesAsync(entities.Select(x => x.Id).ToList(), cancellationToken);
        return entities.ToList();
    }

    public async ValueTask<MySqlBulkCopyResult> BulkInsertAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var table = new System.Data.DataTable(_tableName);
        var properties = entities[0].GetPropertyInfos();
        var ignoreAttributes = BaseAttributes.GetCommonIgnoreAttribute();

        // Thêm column vào datatable
        foreach (var prop in properties)
        {
            // Ignore các properties có attribute không thêm vào table
            var allowProp = ignoreAttributes.Find(att => Attribute.IsDefined(prop, att)) == null;
            if (allowProp)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
        }

        // Thêm value vào từng row datatable
        foreach (var entity in entities)
        {
            var row = table.NewRow();
            var propertiesT = entity.GetPropertyInfos();
            foreach (var prop in propertiesT)
            {
                // Ignore các properties có attribute không thêm vào table
                var allowProp = ignoreAttributes.Find(att => Attribute.IsDefined(prop, att)) == null;
                if (allowProp)
                {
                    row[prop.Name] = prop.GetValue(entity) ?? DBNull.Value;
                }
            }
            table.Rows.Add(row);
        }

        var result = await _dbConnection.WriteToServerAsync(table, entities, cancellationToken: cancellationToken);
        return result;
    }

    protected virtual void BeforeSave(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (entity is IDateTracking dateTracking)
            {
                dateTracking.CreatedDate = DateHelper.Now;
                dateTracking.LastModifiedDate = null;
            }

            if (entity is IUserTracking userTracking)
            {
                userTracking.CreatedBy = _currentUser.Context.OwnerId;
                userTracking.LastModifiedBy = null;
            }

            if (entity is ISoftDelete softDelete)
            {
                softDelete.DeletedDate = null;
                softDelete.DeletedBy = null;
                softDelete.IsDeleted = false;
            }

            if (entity is IPersonalizeEntity personalizeEntity)
            {
                personalizeEntity.OwnerId = _currentUser.Context.OwnerId;
            }
        }
    }
    
    protected virtual void BeforeUpdate(TEntity entity)
    {
        if (entity is IDateTracking dateTracking)
        {
            dateTracking.CreatedDate = DateHelper.Now;
            dateTracking.LastModifiedDate =  DateHelper.Now;
        }

        if (entity is IUserTracking userTracking)
        {
            userTracking.CreatedBy = _currentUser.Context.OwnerId;
            userTracking.LastModifiedBy = _currentUser.Context.OwnerId;
        }
        
        if (entity is IPersonalizeEntity personalizeEntity)
        {
            personalizeEntity.OwnerId = _currentUser.Context.OwnerId;
        }
        
    }
    
    protected virtual void BeforeDelete(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            
        }
    }

    protected virtual async Task ClearCacheWhenChangesAsync(List<TKey> ids, CancellationToken cancellationToken = default)
    {
        var tasks = new List<Task>();
        var fullRecordKey = _isSystemTable ? BaseCacheKeys.GetSystemFullRecordsKey(_tableName) : BaseCacheKeys.GetFullRecordsKey(_tableName, _currentUser.Context.OwnerId);
        tasks.Add(_sequenceCaching.DeleteAsync(fullRecordKey));

        if (ids != null && ids.Any())
        {
            foreach (var id in ids)
            {
                var recordByIdKey = _isSystemTable ? BaseCacheKeys.GetSystemRecordByIdKey(_tableName, id) : BaseCacheKeys.GetRecordByIdKey(_tableName, id, _currentUser.Context.OwnerId);
                tasks.Add(_sequenceCaching.DeleteAsync(recordByIdKey));
            }
        }
        
        await Task.WhenAll(tasks);
    }
    
}