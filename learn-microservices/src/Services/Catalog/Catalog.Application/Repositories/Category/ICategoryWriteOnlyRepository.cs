using Catalog.Application.Persistence;
using Catalog.Domain.Entities;
using SharedKernel.Application.Repositories;

namespace Catalog.Application.Repositories;


public interface ICategoryWriteOnlyRepository : IEfCoreWriteOnlyRepository<Category, IApplicationDbContext>
{
    Task<Category> CreateCategoryAsync(Category category, CancellationToken cancellationToken = default);
    
    Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken = default);
    
    Task<Guid> DeleteCategoryAsync(Category category, CancellationToken cancellationToken = default);

    Task DeleteMultipleSupplierAsync(IList<Category> categories, CancellationToken cancellationToken = default);
    
}