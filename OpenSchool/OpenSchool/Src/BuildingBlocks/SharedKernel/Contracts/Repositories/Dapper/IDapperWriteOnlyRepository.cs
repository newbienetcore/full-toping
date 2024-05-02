using MySqlConnector;
using SharedKernel.Domain;
using SharedKernel.UnitOfWork;

namespace SharedKernel.Contracts.Repositories;

public interface IDapperWriteOnlyRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
{
    IUnitOfWork UnitOfWork { get; }

    Task<TEntity> SaveAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<List<TEntity>> SaveAsync(List<TEntity> entities, CancellationToken cancellationToken = default);

    Task<TEntity> UpdateAsync(TEntity entity, IList<string> updateFields = default, CancellationToken cancellationToken = default);

    Task<List<TEntity>> DeleteAsync(List<TKey> ids, CancellationToken cancellationToken = default);

    ValueTask<MySqlBulkCopyResult> BulkInsertAsync(List<TEntity> entities, CancellationToken cancellationToken = default);
}