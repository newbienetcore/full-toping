using SharedKernel.Domain;
using SharedKernel.UnitOfWork;

namespace SharedKernel.EFCore;

public interface ICoreDbContext : IUnitOfWork
{
    Task BulkInsertAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : EntityAuditBase;

    Task BulkUpdateAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class;
    
    Task BulkDeleteAsync<TEntity>(IList<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class;
}