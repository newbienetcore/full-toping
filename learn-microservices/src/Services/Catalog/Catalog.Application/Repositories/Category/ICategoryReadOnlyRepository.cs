using Catalog.Application.DTOs;
using Catalog.Application.Persistence;
using Catalog.Domain.Entities;
using SharedKernel.Application;
using SharedKernel.Application.Repositories;

namespace Catalog.Application.Repositories;


public interface ICategoryReadOnlyRepository : IEfCoreReadOnlyRepository<Category, IApplicationDbContext>
{
    Task<IList<CategoryDto>> GetAllCategoryAsync(CancellationToken cancellationToken = default);
    
    Task<IList<CategorySummaryDto>> GetCategoryHierarchyAsync(Category category, CancellationToken cancellationToken = default);
    
    Task<IList<Category>> GetListCategoryByIdsAsync(IList<Guid> categoryIds, CancellationToken cancellationToken = default);

    Task<Category?> GetCategoryByIdAsync(object categoryId, CancellationToken cancellationToken = default);
    
    Task<string> IsDuplicate(Guid? categoryId, string name, CancellationToken cancellationToken = default);

    Task<bool> IsParentCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
    
    Task<bool> HasProductCategoriesAsync(Guid categoryId, CancellationToken cancellationToken = default);
    
    Task<IPagedList<CategoryDto>> GetPagingResultAsync(PagingRequest request, CancellationToken cancellationToken = default);
    
    Task<Category?> GetCategoryByAliasAsync(string alias, CancellationToken cancellationToken = default);
    
    Task<IList<CategoryNavigationDto>> GetCategoryNavigationAsync(CancellationToken cancellationToken = default);
}