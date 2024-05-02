using AutoMapper;
using AutoMapper.QueryableExtensions;
using Caching;
using Catalog.Application.DTOs;
using Catalog.Application.Repositories;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver.Linq;
using SharedKernel.Libraries;

namespace Catalog.Infrastructure.Repositories;

public class CategoryReadOnlyRepository : EfCoreReadOnlyRepository<Category, ApplicationDbContext>, ICategoryReadOnlyRepository
{
    private readonly IFileService _fileService;
    public CategoryReadOnlyRepository(
        ApplicationDbContext dbContext,
        ICurrentUser currentUser,
        ISequenceCaching sequenceCaching,
        IServiceProvider provider,
        IFileService fileService
    ) : base(dbContext, currentUser, sequenceCaching, provider)
    {
        _fileService = fileService;
    }

    public async Task<IList<CategoryDto>> GetAllCategoryAsync(CancellationToken cancellationToken = default)
    {
        // Handle caching ...
        
        var mapper = _provider.GetRequiredService<IMapper>();
        var categories = await FindByCondition(e => e.Status)
            .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return categories;

    }

    public async Task<IList<CategorySummaryDto>> GetCategoryHierarchyAsync(Category category,
        CancellationToken cancellationToken = default)
    {
        var categories = 
            await FindByCondition(e => (e.Path.StartsWith(category.Path) || category.Path.StartsWith(e.Path)) && e.Status)
            .OrderBy(e => e.Level)
            .Select(e => new CategorySummaryDto()
            {
                Id = e.Id,
                Name = e.Name,
                Alias = e.Alias,
                Description = e.Description,
                FileName = e.FileName,
                Url = _fileService.GetFileUrl(e.FileName),
                OrderNumber = e.OrderNumber,
                Level = e.Level,
                ParentId = e.ParentId,
                IsSelected = e.Id != category.Id
            })
            .ToListAsync(cancellationToken);

        return categories;
    }

    public async Task<IList<Category>> GetListCategoryByIdsAsync(IList<Guid> categoryIds,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(e => categoryIds.Contains(e.Id) && e.Status).ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetCategoryByIdAsync(object categoryId, CancellationToken cancellationToken = default)
    {
        return await GetByIdWithCachingAsync(categoryId, cancellationToken);
    }

    public async Task<string> IsDuplicate(Guid? categoryId, string name,
        CancellationToken cancellationToken = default)
    {
        var duplicateSupplier = await _dbSet.FirstOrDefaultAsync(
            e => (categoryId == null || e.Id != categoryId) && e.Name == name && e.Status,
            cancellationToken);

        if (duplicateSupplier is null)
        {
            return string.Empty;
        }

        if (duplicateSupplier.Name == name)
        {
            return "supplier_is_duplicate_name";
        }

        return string.Empty;
    }

    public async Task<bool> IsParentCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(e => e.ParentId == categoryId && e.Status, cancellationToken);
    }

    public async Task<bool> HasProductCategoriesAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ProductCategories.AnyAsync(e => e.CategoryId == categoryId, cancellationToken);
    }

    public async Task<IPagedList<CategoryDto>> GetPagingResultAsync(PagingRequest request, CancellationToken cancellationToken = default)
    {
        var mapper = _provider.GetRequiredService<IMapper>();

        var result = await _dbSet
            .WhereIf(!string.IsNullOrEmpty(request.SearchString),
                e => e.Name.Contains(request.SearchString) || e.Description.Contains(request.SearchString))
            .ApplySorting(request.Sorts)
            .AsNoTracking()
            .ToPagedListAsync<Category, CategoryDto>(
                mapper,
                request.Page,
                request.Size,
                request.IndexFrom,
                cancellationToken);

        return result;
    }

    public async Task<Category?> GetCategoryByAliasAsync(string alias, CancellationToken cancellationToken = default)
    {
        string key = BaseCacheKeys.GetSystemRecordByIdKey(_tableName, alias);

        var cacheResult = await _sequenceCaching.GetAsync<Category>(key, cancellationToken: cancellationToken);
        if (cacheResult is not null)
        {
            return cacheResult;
        }

        var supplier =
            await _dbSet.FirstOrDefaultIfAsync(!string.IsNullOrEmpty(alias), e => e.Alias == alias && e.Status,
                cancellationToken);

        if (supplier is not null)
        {
            await _sequenceCaching.SetAsync(key, supplier, cancellationToken: cancellationToken);
        }

        return supplier;
    }

    public async Task<IList<CategoryNavigationDto>> GetCategoryNavigationAsync(CancellationToken cancellationToken = default)
    {
        string key = BaseCacheKeys.GetSystemFullRecordsKey(_tableName);
        var navigations =
            await _sequenceCaching.GetAsync<IList<CategoryNavigationDto>>(key, cancellationToken: cancellationToken);

        if (navigations != null)
        {
            return navigations;
        }

        var allCategories = await _dbSet.Where(x => x.Status).ToListAsync(cancellationToken);
        var roots = allCategories.Where(x => !x.ParentId.HasValue)
            .OrderBy(x => x.OrderNumber).ToList();

        if (!roots.Any())
        {
            return new List<CategoryNavigationDto>();
        }

        navigations = roots.Select(root => MapCategoryToDto(root, allCategories)).ToList();

        await _sequenceCaching.SetAsync(key, navigations, onlyUseType: CachingType.Redis,
            cancellationToken: cancellationToken);

        return navigations;
    }

    private CategoryNavigationDto MapCategoryToDto(Category category, IEnumerable<Category> allCategories)
    {
        var childrenCategories = allCategories.Where(x => x.ParentId == category.Id).OrderBy(x => x.OrderNumber);
        var childrenDtos = childrenCategories.Select(child => MapCategoryToDto(child, allCategories)).ToList();

        return new CategoryNavigationDto
        {
            Id = category.Id,
            ParentId = category.ParentId,
            Name = category.Name,
            Alias = category.Alias,
            Description = category.Description,
            Level = category.Level,
            FileName = category.FileName,
            OrderNumber = category.OrderNumber,
            Status = category.Status,
            Path = category.Path,
            Children = childrenDtos
        };
    }
}