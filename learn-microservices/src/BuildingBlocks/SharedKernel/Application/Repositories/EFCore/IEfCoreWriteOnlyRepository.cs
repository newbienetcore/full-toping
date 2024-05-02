using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain;
using SharedKernel.Persistence;
using SharedKernel.UnitOfWork;

namespace SharedKernel.Application.Repositories;

public interface IEfCoreWriteOnlyRepository<TEntity, TDbContext> : IEfCoreReadOnlyRepository<TEntity, TDbContext>
    where TEntity : BaseEntity
    where TDbContext : IAppDbContext
{
    IUnitOfWork UnitOfWork { get; }
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<IList<TEntity>> InsertAsync(IList<TEntity> entities, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(IList<TEntity> entities, CancellationToken cancellationToken);
}