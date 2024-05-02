using System.Linq.Expressions;
using Caching;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Application;
using SharedKernel.Application.Repositories;
using SharedKernel.Auth;
using SharedKernel.Domain;
using SharedKernel.Persistence;

namespace SharedKernel.Infrastructures.Repositories;

public class EfCoreReadOnlyRepository<TEntity, TDbContext> : IEfCoreReadOnlyRepository<TEntity, TDbContext>
    where TEntity :  BaseEntity
    where TDbContext : AppDbContext
{
    protected readonly TDbContext _dbContext;
    protected readonly ICurrentUser _currentUser;
    protected readonly ISequenceCaching _sequenceCaching;
    protected readonly IServiceProvider _provider;
    protected readonly DbSet<TEntity> _dbSet;
    protected readonly string _tableName;

    public EfCoreReadOnlyRepository(
        TDbContext dbContext, 
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching, 
        IServiceProvider provider)
    {
        _dbContext = dbContext;
        _currentUser = currentUser;
        _sequenceCaching = sequenceCaching;
        _provider = provider;
        
        _tableName = ((TEntity)Activator.CreateInstance(typeof(TEntity))).GetTableName();
        _dbSet = dbContext.Set<TEntity>();
    }
    
    public IQueryable<TEntity> FindAll(bool trackChanges = false)
    {
        var queryable = !trackChanges ? _dbSet.AsNoTracking().Where(x => !x.IsDeleted) : _dbSet.Where(x => !x.IsDeleted);
        
        return queryable;
    }

    public IQueryable<TEntity> FindAll(bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var queryable = FindAll(trackChanges);
        queryable = includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));
        return queryable;
    }

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false)
    {
        var queryable = !trackChanges ? _dbSet.AsNoTracking().Where(x => !x.IsDeleted).Where(expression) : _dbSet.Where(x => !x.IsDeleted).Where(expression);
        
        return queryable;
    }
    
    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var queryable = FindByCondition(expression, trackChanges);
        queryable = includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));
        return queryable;
    }

    public async Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    { 
        return await FindByCondition(x => x.Id == (Guid)id, trackChanges: true)
            .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<TEntity?> GetByIdWithCachingAsync(object id, CancellationToken cancellationToken = default)
    {
        var cacheResult = await GetByIdCacheAsync(id, cancellationToken);
        if (cacheResult is not null)
        {
            return cacheResult;
        }
        
        var entity = await FindByCondition(x => x.Id == (Guid)id)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity is not null)
        {
            string key = BaseCacheKeys.GetSystemRecordByIdKey(_tableName, id);
            await _sequenceCaching.SetAsync(key, entity,cancellationToken: cancellationToken);
        }

        return entity;
    }

    public async Task<TEntity?> GetByIdAsync(object id,  CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
    {
         return await FindByCondition(x => x.Id == (Guid)id, trackChanges: false, includeProperties)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    #region [CACHE]
    
    public virtual async Task<List<TEntity>> GetAllCacheAsync(CancellationToken cancellationToken = default)
    {
        string key =  BaseCacheKeys.GetSystemFullRecordsKey(_tableName);
        
        return await _sequenceCaching.GetAsync<List<TEntity>>(key, cancellationToken: cancellationToken);
    }
    
    public virtual async Task<TEntity?> GetByIdCacheAsync(object id, CancellationToken cancellationToken)
    {
        string key = BaseCacheKeys.GetSystemRecordByIdKey(_tableName, id);
        
        return await _sequenceCaching.GetAsync<TEntity>(key, cancellationToken: cancellationToken);
    }
    
    #endregion
    
}