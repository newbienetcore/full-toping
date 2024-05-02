using Caching;
using MassTransit.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using SharedKernel.Auth;
using SharedKernel.Contracts;
using SharedKernel.Contracts.Repositories;
using SharedKernel.Domain;
using SharedKernel.Libraries;
using SharedKernel.MySQL;
using SharedKernel.Properties;
using SharedKernel.Runtime.Exceptions;

namespace SharedKernel.Infrastructures;

public class DapperReadOnlyRepository<TEntity, TKey> : IDapperReadOnlyRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
{
    protected IDbConnection _dbConnection;
    protected readonly string _tableName;
    protected readonly ICurrentUser _currentUser;
    protected readonly ISequenceCaching _sequenceCaching;
    protected readonly IServiceProvider _provider;
    protected readonly bool _isSystemTable;

    public DapperReadOnlyRepository(
        IDbConnection dbConnection, 
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching, 
        IServiceProvider provider)
    {
        _dbConnection = dbConnection;
        _currentUser = currentUser;
        _sequenceCaching = sequenceCaching;
        _tableName = ((TEntity)Activator.CreateInstance(typeof(TEntity))).GetTableName();
        _provider = provider;
        _isSystemTable = typeof(TEntity).GetProperty("OwnerId") == null;
    }
    
    public virtual async Task<IEnumerable<TResult>> GetAllAsync<TResult>(CancellationToken cancellationToken = default)
    {
        var cacheResult = await GetAllCacheAsync<TResult>(cancellationToken);
        if (cacheResult.Value != null && cacheResult.Value.Any())
        {
            return cacheResult.Value;
        }

        var cmd = $"SELECT * FROM {_tableName} as T WHERE 1=1";
        if (typeof(TEntity).HasInterface<IPersonalizeEntity>())
        {
            cmd += $" AND T.OwnerId = '{_currentUser.Context.OwnerId}'";
        }
        if (typeof(TEntity).HasInterface<ISoftDelete>())
        {
            cmd += " AND T.IsDeleted = 0";
        }
        if (typeof(TEntity).HasInterface<IDateTracking>())
        {
            cmd += $" ORDER BY CASE WHEN T.LastModifiedDate > T.CreatedDate THEN T.LastModifiedDate ELSE T.CreatedDate END DESC";
        }
       
        var result = await _dbConnection.QueryAsync<TResult>(cmd);
        if (result.Any())
        {
            await _sequenceCaching.SetAsync(cacheResult.Key, result, TimeSpan.FromDays(7));
        }
        return result;
    }
    
    public virtual async Task<TResult> GetByIdAsync<TResult>(TKey id, CancellationToken cancellationToken = default)
    {
        var cacheResult = await GetByIdCacheAsync<TResult>(id, cancellationToken);
        if (cacheResult.Value != null)
        {
            return cacheResult.Value;
        }

        var cmd = $"SELECT * FROM {_tableName} as T WHERE T.Id = @Id";
        if (typeof(TEntity).HasInterface<IPersonalizeEntity>())
        {
            cmd += $" AND T.OwnerId = '{_currentUser.Context.OwnerId}'";
        }
        if (typeof(TEntity).HasInterface<ISoftDelete>())
        {
            cmd += " AND T.IsDeleted = 0";
        }
       
        var result = await _dbConnection.QuerySingleOrDefaultAsync<TResult>(cmd, new { Id = id });
        if (result != null)
        {
            await _sequenceCaching.SetAsync(cacheResult.Key, result, TimeSpan.FromDays(7));
        }
        return result;
    }

   public virtual async Task<IPagedList<TResult>> GetPagingAsync<TResult>(PagingRequest request, CancellationToken cancellationToken)
    {
        var cmd = $"SELECT * FROM {_tableName} as T WHERE 1 = 1";
        var countCmd = $"SELECT Count(Id) FROM {_tableName} as T WHERE 1 = 1";

        if (typeof(TEntity).HasInterface<IPersonalizeEntity>())
        {
            cmd += $" AND T.OwnerId = '{_currentUser.Context.OwnerId}'";
            countCmd += $" AND T.OwnerId = '{_currentUser.Context.OwnerId}'";
        }
        
        if (typeof(TEntity).HasInterface<ISoftDelete>())
        {
            cmd += " AND T.IsDeleted = 0";
            countCmd += " AND T.IsDeleted = 0";
        }

        // Filter
        var param = new Dictionary<string, object>();
        if (request.Filter != null && request.Filter.Fields != null && request.Filter.Fields.Any())
        {
            var formula = request.Filter.Formula;
            for (int i = 0; i < request.Filter.Fields.Count; i++)
            {
                var field = request.Filter.Fields[i];
                var property = typeof(TEntity).GetProperty(field.FieldName);
                if (property == null || !Attribute.IsDefined(property, typeof(FilterableAttribute)))
                {
                    var localizer = _provider.GetRequiredService<IStringLocalizer<Resources>>();
                    throw new BadRequestException(localizer["repository_filter_is_invalid"].Value);
                }

                var hasUnicode = field.Value.HasUnicode();
                var replaceValue = $" T.{field.FieldName} {field.GetOperatorWithValue(out string paramName, hasUnicode)}";
                formula = formula.Replace("{" + i + "}", replaceValue);
                param[paramName] = field.Value;
            }
            cmd += $" AND ({formula}) ";
            countCmd += $" AND ({formula}) ";
        }

        // Order by
        var limit = $"LIMIT {request.Offset}, {request.Size}";
        if (request.Sorts != null && request.Sorts.Any())
        {

            var tmp = new List<string>();
            foreach (var sort in request.Sorts)
            {
                if (Secure.DetectSqlInjection(sort.FieldName))
                {
                    throw new SqlInjectionException();
                }
                tmp.Add($" T.{sort.FieldName} {(sort.SortAscending ? "ASC" : "DESC")} ");
            }
            var order = $" ORDER BY {string.Join(",", tmp)} ";
            cmd += $" {order} {limit}";
        }
        else
        {
            
            if (typeof(TEntity).HasInterface<IDateTracking>())
            {
                cmd += $" ORDER BY CASE WHEN T.LastModifiedDate > T.CreatedDate THEN T.LastModifiedDate ELSE T.CreatedDate END DESC {limit}";
            }
            else
            {
                cmd += $" {limit}";
            }
            
        }

        var dataTask = _dbConnection.QueryAsync<TResult>(cmd, param);
        var countTask = _dbConnection.QuerySingleOrDefaultAsync<int>(countCmd, param);
        
        await Task.WhenAll(dataTask, countTask);
        
        int totalCount = await countTask;
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.Size);
        return new PagedList<TResult>(
            request.Page,
            request.Size,
            request.IndexFrom,
            totalCount,
            totalPages,
            await dataTask);
    }
   
    public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        var cmd = $"SELECT COUNT(*) FROM {_tableName} as T WHERE 1=1";
        if (typeof(TEntity).HasInterface<IPersonalizeEntity>()) cmd += $" AND T.OwnerId = '{_currentUser.Context.OwnerId}'";
        if (typeof(TEntity).HasInterface<ISoftDelete>()) cmd += $" AND T.IsDeleted = 0";

        return await _dbConnection.QuerySingleOrDefaultAsync<long>(cmd);
    }

    #region Cache

    public virtual async Task<CacheResult<List<TResult>>> GetAllCacheAsync<TResult>(CancellationToken cancellationToken = default)
    {
        string key = string.Empty;
        if (_isSystemTable)
        {
            key = BaseCacheKeys.GetSystemFullRecordsKey(_tableName);
        }
        else
        {
            key =  BaseCacheKeys.GetFullRecordsKey(_tableName, _currentUser.Context.OwnerId);
        }
        return new CacheResult<List<TResult>>(key, await _sequenceCaching.GetAsync<List<TResult>>(key));
    }

    public virtual async Task<CacheResult<TResult>> GetByIdCacheAsync<TResult>(TKey id, CancellationToken cancellationToken = default)
    {
        string key = string.Empty;
        if (_isSystemTable)
        {
            key = BaseCacheKeys.GetSystemRecordByIdKey(_tableName, id);
        }
        else
        {
            key =  BaseCacheKeys.GetRecordByIdKey(_tableName, id, _currentUser.Context.OwnerId);
        }
        return new CacheResult<TResult>(key, await _sequenceCaching.GetAsync<TResult>(key));
    }
    
    #endregion
}