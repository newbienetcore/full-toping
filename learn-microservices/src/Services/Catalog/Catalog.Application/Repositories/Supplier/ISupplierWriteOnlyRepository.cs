using System.Collections;
using Catalog.Application.DTOs;
using Catalog.Application.Persistence;
using Catalog.Domain.Entities;
using SharedKernel.Application.Repositories;

namespace Catalog.Application.Repositories;

public interface ISupplierWriteOnlyRepository : IEfCoreWriteOnlyRepository<Supplier, IApplicationDbContext>
{
    Task UpdateSupplierAsync(Supplier supplier, CancellationToken cancellationToken = default);
    
    Task<Guid> DeleteSupplierAsync(Supplier supplier, CancellationToken cancellationToken = default);

    Task DeleteMultipleSupplierAsync(IList<Supplier> suppliers, CancellationToken cancellationToken = default);
    
}