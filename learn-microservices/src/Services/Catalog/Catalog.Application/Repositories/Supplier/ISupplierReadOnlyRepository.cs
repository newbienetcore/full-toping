using System.Collections;
using Catalog.Application.DTOs;
using Catalog.Application.Persistence;
using Catalog.Domain.Entities;
using SharedKernel.Application;
using SharedKernel.Application.Repositories;

namespace Catalog.Application.Repositories;

public interface ISupplierReadOnlyRepository :  IEfCoreReadOnlyRepository<Supplier, IApplicationDbContext>
{
    Task<IList<Supplier>> GetListSupplierByIdsAsync(IList<Guid> supplierIds, CancellationToken cancellationToken = default);
    
    Task<string> IsDuplicate(Guid? id, string email, string phone, string name, CancellationToken cancellationToken = default);
    
    Task<IPagedList<SupplierDto>> GetPagingResultAsync(PagingRequest request, CancellationToken cancellationToken = default);
    
    Task<Supplier?> GetSupplierByAliasWithCachingAsync(string alias, CancellationToken cancellationToken = default);
}