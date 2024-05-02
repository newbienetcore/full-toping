using System.Collections;
using Caching;
using Catalog.Application.Repositories;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;

namespace Catalog.Infrastructure.Repositories;

public class SupplierWriteOnlyRepository : EfCoreWriteOnlyRepository<Supplier, ApplicationDbContext>, ISupplierWriteOnlyRepository
{
    public SupplierWriteOnlyRepository(
        ApplicationDbContext dbContext, 
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching, 
        IServiceProvider provider
        ) : base(dbContext, currentUser, sequenceCaching, provider)
    {
        
    }

    public async Task UpdateSupplierAsync(Supplier supplier, CancellationToken cancellationToken = default)
    {
        string key = BaseCacheKeys.GetSystemRecordByIdKey(_tableName, supplier.Alias);
        await Task.WhenAll(new List<Task>()
        {
            UpdateAsync(supplier, cancellationToken),
            _sequenceCaching.DeleteAsync(key, cancellationToken: cancellationToken)
        });
    }

    public async Task<Guid> DeleteSupplierAsync(Supplier supplier, CancellationToken cancellationToken = default)
    {
        string key = BaseCacheKeys.GetSystemRecordByIdKey(_tableName, supplier.Alias);
        await Task.WhenAll(new List<Task>()
        {
            DeleteAsync(supplier, cancellationToken),
            _sequenceCaching.DeleteAsync(key, cancellationToken: cancellationToken)
        });
        
        return supplier.Id;
    }
    
    public async Task DeleteMultipleSupplierAsync(IList<Supplier> suppliers, CancellationToken cancellationToken = default)
    {
        var tasks = new List<Task>();
        
        tasks.Add(DeleteAsync(suppliers, cancellationToken));
        
        foreach (var supplier in suppliers)
        {
            var recordByaAliasKey = BaseCacheKeys.GetSystemRecordByIdKey(_tableName, supplier.Alias);
            tasks.Add(_sequenceCaching.DeleteAsync(recordByaAliasKey, cancellationToken: cancellationToken));
        }
        
        await Task.WhenAll(tasks);
    }
}