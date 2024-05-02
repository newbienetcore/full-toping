using System.Linq.Expressions;
using Caching;
using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Auth;
using SharedKernel.Contracts;
using SharedKernel.Contracts.Repositories;
using SharedKernel.Domain;
using SharedKernel.EFCore;

namespace SharedKernel.Infrastructures;

public class ReadOnlyRepository<TEntity, TKey, TDbContext> : IReadOnlyRepository<TEntity, TKey, TDbContext>
    where TEntity :  EntityBase<TKey>
    where TDbContext : CoreDbContext
{
    protected readonly TDbContext _context;
    protected readonly ICurrentUser _currentUser;
    protected readonly ISequenceCaching _sequenceCaching;
    protected readonly DbSet<TEntity> _dbSet;
    protected readonly string _tableName;
    public readonly bool _isSystemTable;
    
    public ReadOnlyRepository(
        TDbContext context, 
        ICurrentUser currentUser,
        ISequenceCaching sequenceCaching)
    {
        _context = context;
        _currentUser = currentUser;
        _sequenceCaching = sequenceCaching;
        _tableName = ((TEntity)Activator.CreateInstance(typeof(TEntity)))?.GetTableName();
        _dbSet = _context.Set<TEntity>();
        _isSystemTable =  typeof(TEntity).HasInterface<IPersonalizeEntity>() == null;
    }
    
    public virtual IQueryable<TEntity> FindAll(bool trackChanges = false)
    {
        return !trackChanges ? _dbSet.AsNoTracking() : _dbSet;
    }

    public virtual IQueryable<TEntity> FindAll(bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var queryable = FindAll(trackChanges);
        queryable = includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));
        return queryable;
    }

    public virtual IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false)
    {
        var queryable = !trackChanges ? _dbSet.AsNoTracking().Where(expression) : _dbSet.Where(expression);
        return queryable;
    }

    public virtual IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges = false, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var items = FindByCondition(expression, trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await GetByIdAsync(id, cancellationToken: cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return await FindByCondition(x => x.Id.Equals(id), trackChanges: false, includeProperties).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdWithCachingAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var cacheResult = await GetByIdCacheAsync(id, cancellationToken);
        if (cacheResult.Value != null)
        {
            return cacheResult.Value;
        }
        
        var entity = await FindByCondition(x => x.Id.Equals(id)).FirstOrDefaultAsync(cancellationToken);

        if (entity is not null)
        {
            await _sequenceCaching.SetAsync(cacheResult.Key, entity);
        }

        return entity;
    }

    public virtual async Task<CacheResult<TEntity>> GetByIdCacheAsync(TKey id, CancellationToken cancellationToken = default)
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
        
        return new CacheResult<TEntity>(key, await _sequenceCaching.GetAsync<TEntity>(key));
    }
}