using SharedKernel.Domain;
using SharedKernel.EFCore;
using SharedKernel.UnitOfWork;

namespace SharedKernel.Contracts.Repositories;

public interface IEFCoreWriteOnlyRepository<TEntity, in TKey ,TDbContext> : IEFCoreReadOnlyRepository<TEntity, TKey ,TDbContext>
    where TEntity : EntityBase<TKey>
    where TDbContext : CoreDbContext
{
    IUnitOfWork UnitOfWork { get; }
    
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    Task<IList<TEntity>> InsertAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);
    
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);
}