using SharedKernel.Domain;

namespace SharedKernel.Contracts.Repositories;

public interface IDapperReadOnlyRepository<TEntity, in TKey> where TEntity : EntityBase<TKey>
{
    #region Cache
    Task<CacheResult<List<TResult>>> GetAllCacheAsync<TResult>(CancellationToken cancellationToken = default);

    Task<CacheResult<TResult>> GetByIdCacheAsync<TResult>(TKey id, CancellationToken cancellationToken = default);
    
    #endregion
    
    
    Task<IEnumerable<TResult>> GetAllAsync<TResult>(CancellationToken cancellationToken = default);

    Task<TResult> GetByIdAsync<TResult>(TKey id, CancellationToken cancellationToken = default);

    Task<IPagedList<TResult>> GetPagingAsync<TResult>(PagingRequest request, CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(CancellationToken cancellationToken = default);
}