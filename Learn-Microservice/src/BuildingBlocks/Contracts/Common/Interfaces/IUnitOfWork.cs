using Contracts.Domains;
using Microsoft.EntityFrameworkCore;

namespace Contracts.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    bool SaveChanges();
    
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    void Rollback();
    
    Task RollbackAsync(CancellationToken cancellationToken = default);
    
    void Commit();
    
    Task CommitAsync(CancellationToken cancellationToken = default);
    
    void BeginTransaction();

    IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : EntityBase<TKey>;

}

public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    
}