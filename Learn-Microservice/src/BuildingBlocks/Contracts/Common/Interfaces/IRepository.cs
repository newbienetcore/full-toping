using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Contracts.Common.Interfaces;

public interface IRepository<TEntity, TKey, TContext> : IRepositoryQueryBase<TEntity, TKey, TContext>
    where TEntity : EntityBase<TKey>
    where TContext : DbContext
{
    TKey Insert(TEntity entity);
        
    IList<TKey> Insert(IEnumerable<TEntity> entities);

    Task<TKey> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
        
    Task<IList<TKey>> InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    void Update(TEntity entity);
        
    void Update(IEnumerable<TEntity> entities);

    void Delete(TEntity entity);

    void Delete(IEnumerable<TEntity> entities);

    bool SaveChanges();
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task EndTransactionAsync(CancellationToken cancellationToken = default);

    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

}

public interface IRepository<TEntity, TKey> : IRepositoryQueryBase<TEntity, TKey>
    where TEntity : EntityBase<TKey>
{
    TKey Insert(TEntity entity);
        
    IList<TKey> Insert(IEnumerable<TEntity> entities);

    Task<TKey> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
        
    Task<IList<TKey>> InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    void Update(TEntity entity);
        
    void Update(IEnumerable<TEntity> entities);

    void Delete(TEntity entity);

    void Delete(IEnumerable<TEntity> entities);

    bool SaveChanges();
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task EndTransactionAsync(CancellationToken cancellationToken = default);

    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

}