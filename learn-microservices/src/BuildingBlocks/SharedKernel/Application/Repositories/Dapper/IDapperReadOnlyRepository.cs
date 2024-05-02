using SharedKernel.Domain;

namespace SharedKernel.Application
{
    public interface IDapperReadOnlyRepository<TEntity> where TEntity : BaseEntity
    {

        #region Cache

        #region Get
        Task<CacheResult<List<TResult>>> GetAllCacheAsync<TResult>(CancellationToken cancellationToken);

        Task<CacheResult<TResult>> GetByIdCacheAsync<TResult>(object id, CancellationToken cancellationToken);
        #endregion

        #region Set

        #endregion

        #endregion

        Task<IEnumerable<TResult>> GetAllAsync<TResult>(CancellationToken cancellationToken);

        Task<TResult> GetByIdAsync<TResult>(object id, CancellationToken cancellationToken);

        Task<IPagedList<TResult>> GetPagingAsync<TResult>(PagingRequest request, CancellationToken cancellationToken);

        Task<int> GetCountAsync(CancellationToken cancellationToken);
    }
}