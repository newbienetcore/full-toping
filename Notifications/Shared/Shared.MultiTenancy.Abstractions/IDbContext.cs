using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Shared.MultiTenancy.Abstractions;

public interface IIChibaDbContextTransaction
{
    IDbContextTransaction CurrentTransaction { get; }
    Task RollbackAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    IExecutionStrategy CreateExecutionStrategy();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DatabaseFacade Database { get; }
}