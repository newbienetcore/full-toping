using Caching;
using MassTransit.Internals;
using Microsoft.Extensions.Localization;
using MySqlConnector;
using SharedKernel.Application;
using SharedKernel.Auth;
using SharedKernel.Domain;
using SharedKernel.Libraries;
using SharedKernel.MySQL;
using SharedKernel.Properties;
using SharedKernel.Runtime.Exceptions;
using SharedKernel.UnitOfWork;

namespace SharedKernel.Infrastructures
{
    public class DapperWriteOnlyRepository<TEntity> : IDapperWriteOnlyRepository<TEntity> where TEntity : IBaseEntity
    {
        protected IDbConnection _dbConnection;
        protected readonly string _tableName;
        protected readonly ICurrentUser CurrentUser;
        protected readonly ISequenceCaching _sequenceCaching;
        protected readonly bool _isSystemTable;
        protected readonly IStringLocalizer<Resources> _localizer;

        public DapperWriteOnlyRepository(
            IDbConnection dbConnection,
            ICurrentUser currentUser,
            ISequenceCaching sequenceCaching,
            IStringLocalizer<Resources> localizer
        )
        {
            _dbConnection = dbConnection;
            CurrentUser = currentUser;
            _sequenceCaching = sequenceCaching;
            _localizer = localizer;
            _tableName = ((TEntity)Activator.CreateInstance(typeof(TEntity))).GetTableName();
        }

        public IUnitOfWork UnitOfWork => _dbConnection;

        public async Task<TEntity> SaveAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return (await SaveAsync(new List<TEntity> { entity }, cancellationToken)).First();
        }

        public virtual async Task<List<TEntity>> SaveAsync(List<TEntity> entities, CancellationToken cancellationToken)
        {
            BeforeSave(entities);
            await BulkInsertAsync(entities, cancellationToken);

            return entities;
        }

        public virtual async ValueTask<MySqlBulkCopyResult> BulkInsertAsync(List<TEntity> entities, CancellationToken cancellationToken)
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

            var result = await _dbConnection.WriteToServerAsync(table, entities, cancellationToken);

            await ClearCacheWhenChangesAsync(null, cancellationToken);
            return result;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, IList<string> updateFields = default, CancellationToken cancellationToken = default)
        {
            var queryCmd = $"SELECT * FROM {_tableName} WHERE Id = @Id AND IsDeleted = 0";
            var currentEntity = await _dbConnection.QuerySingleOrDefaultAsync<TEntity>(queryCmd, new { entity.Id })
                                ?? throw new BadRequestException(_localizer["repository_data_does_not_exist_or_was_deleted"].Value);

            var sqlCommand = $"UPDATE {_tableName} AS T SET ";
            var columnParams = new List<string>();
            var ignoreFields = new string[]
            {
                nameof(BaseEntity.Id),
                nameof(BaseEntity.CreatedDate),
                nameof(BaseEntity.CreatedBy),
                nameof(BaseEntity.IsDeleted),
                nameof(PersonalizedEntity.OwnerId),
            };
            var properties = entity.GetPropertyInfos();

            // Map lại giá trị cũ vào entity
            if (updateFields != null)
            {
                updateFields.Add("LastModifiedDate");
                updateFields.Add("LastModifiedBy");
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
            BeforeUpdate(entity, currentEntity);

            sqlCommand += string.Join(", ", columnParams) + " WHERE T.Id = @Id AND T.IsDeleted = 0";
            if (typeof(TEntity).GetProperty("OwnerId") != null)
            {
                sqlCommand += $" AND IF(OwnerId = '{CurrentUser.Context.OwnerId}', TRUE, IF(CreatedBy = '{CurrentUser.Context.OwnerId}', TRUE, FALSE));";
            }
            else
            {
                sqlCommand += $" AND IF(CreatedBy = '{CurrentUser.Context.OwnerId}', TRUE, FALSE);";
            }

            await _dbConnection.ExecuteAsync(sqlCommand, entity);

            await ClearCacheWhenChangesAsync(new List<object> { entity.Id }, cancellationToken);
            return currentEntity;
        }

        public virtual async Task<List<TEntity>> DeleteAsync(List<string> ids, CancellationToken cancellationToken)
        {
            var joinedIds = string.Join(", ", ids);
            if (Secure.DetectSqlInjection(joinedIds))
            {
                throw new SqlInjectionException();
            }

            var sqlCommand = $"SELECT * FROM {_tableName} AS T WHERE T.Id In ( {joinedIds}) AND T.IsDeleted = 0";
            var deleteCommand = $"UPDATE {_tableName} AS T SET T.IsDeleted = 1, T.DeletedDate = @DeletedDate, T.DeletedBy = @DeletedBy WHERE T.Id IN( {joinedIds} )";

            if (typeof(TEntity).GetProperty("OwnerId") != null)
            {
                sqlCommand += $" AND IF(OwnerId = '{CurrentUser.Context.OwnerId}', TRUE, IF(CreatedBy = '{CurrentUser.Context.OwnerId}', TRUE, FALSE));";
                deleteCommand += $" AND IF(OwnerId = '{CurrentUser.Context.OwnerId}', TRUE, IF(CreatedBy = '{CurrentUser.Context.OwnerId}', TRUE, FALSE));";
            }
            else
            {
                sqlCommand += $" AND IF(CreatedBy = '{CurrentUser.Context.OwnerId}', TRUE, FALSE);";
                deleteCommand += $" AND IF(CreatedBy = '{CurrentUser.Context.OwnerId}', TRUE, FALSE);";
            }

            var entities = await _dbConnection.QueryAsync<TEntity>(sqlCommand);

            if (entities.Any())
            {
                BeforeDelete(entities);
                var param = new BaseEntity
                {
                    DeletedDate = DateHelper.Now,
                    DeletedBy = CurrentUser.Context.OwnerId,
                };
                
                await _dbConnection.ExecuteAsync(deleteCommand, param);
            }
            await ClearCacheWhenChangesAsync(entities.Select(x => (object)x.Id).ToList(), cancellationToken);
            return entities.ToList();
        }

        protected virtual void BeforeSave(IEnumerable<TEntity> entities)
        {
            var batches = entities.ChunkList(1000);
            batches.ToList().ForEach(async entities =>
            {
                entities.ForEach(entity =>
                {
                    entity.Id = Guid.NewGuid();
                    entity.CreatedBy = CurrentUser.Context.OwnerId;
                    entity.CreatedDate = DateHelper.Now;
                    entity.LastModifiedDate = null;
                    entity.LastModifiedBy = null;
                    entity.DeletedDate = null;
                    entity.DeletedBy = null;
                });

                if (batches.Count() > 1)
                {
                    await Task.Delay(69);
                }
            });
            if (typeof(TEntity).HasInterface(typeof(IPersonalizeEntity)))
            {
                foreach (var entity in entities)
                {
                    entity["OwnerId"] = CurrentUser.Context.OwnerId;
                }
            }
        }

        protected virtual void BeforeUpdate(TEntity entity, TEntity oldValue)
        {
            entity.CreatedBy = CurrentUser.Context.OwnerId;
            entity.LastModifiedDate = DateHelper.Now;
            entity.LastModifiedBy = CurrentUser.Context.OwnerId;
            if (typeof(TEntity).HasInterface(typeof(IPersonalizeEntity)))
            {
                entity["OwnerId"] = CurrentUser.Context.OwnerId;
            }
        }

        protected virtual void BeforeDelete(IEnumerable<TEntity> entities)
        {
            
        }

        protected virtual async Task ClearCacheWhenChangesAsync(List<object> ids, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            var fullRecordKey = BaseCacheKeys.GetSystemFullRecordsKey(_tableName);
            tasks.Add(_sequenceCaching.DeleteAsync(fullRecordKey, cancellationToken: cancellationToken));

            if (ids != null && ids.Any())
            {
                foreach (var id in ids)
                {
                    var recordByIdKey = BaseCacheKeys.GetSystemRecordByIdKey(_tableName, id);
                    tasks.Add(_sequenceCaching.DeleteAsync(recordByIdKey, cancellationToken: cancellationToken));
                }
            }
            await Task.WhenAll(tasks);
        }
    }
}
