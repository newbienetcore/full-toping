using System.Linq.Expressions;
using SharedKernel.Domain;
using SharedKernel.EFCore;

namespace SharedKernel.Contracts.Repositories;

public interface IEFCoreReadOnlyRepository<TEntity, in TKey, TDbContext> 
    where TEntity : EntityBase<TKey>
    where TDbContext : CoreDbContext
{
    IQueryable<TEntity> FindAll(bool trackChanges = false);
    
    IQueryable<TEntity> FindAll(bool trackChanges = false, 
        params Expression<Func<TEntity, object>>[] includeProperties);
    
    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression,
        bool trackChanges = false);
    
    IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, 
        bool trackChanges = false,
        params Expression<Func<TEntity, object>>[] includeProperties);
    
    Task<TEntity?> GetByIdAsync(TKey id, 
        CancellationToken cancellationToken = default);
    
    Task<TEntity?> GetByIdAsync(TKey id, 
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includeProperties);
    
    Task<TEntity?> GetByIdWithCachingAsync(TKey id, 
        CancellationToken cancellationToken = default);
    
    #region Cache
    Task<CacheResult<TEntity>> GetByIdCacheAsync(TKey id, 
        CancellationToken cancellationToken = default);
    
    #endregion
}