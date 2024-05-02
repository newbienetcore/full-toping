using Caching;
using Catalog.Application.Repositories;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;

namespace Catalog.Infrastructure.Repositories;

public class CategoryWriteOnlyRepository : EfCoreWriteOnlyRepository<Category, ApplicationDbContext>, ICategoryWriteOnlyRepository
{
    public CategoryWriteOnlyRepository(
        ApplicationDbContext dbContext,
        ICurrentUser currentUser,
        ISequenceCaching sequenceCaching,
        IServiceProvider provider
    ) : base(dbContext, currentUser, sequenceCaching, provider)
    {
    }
    
    public async Task<Category> CreateCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
         string key = BaseCacheKeys.GetSystemFullRecordsKey(_tableName);
         await Task.WhenAll(new List<Task>()
         {
             InsertAsync(category, cancellationToken),
             _sequenceCaching.DeleteAsync(key, cancellationToken: cancellationToken)
         });
         return category;
    }

    public async Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        string key = BaseCacheKeys.GetSystemRecordByIdKey(_tableName, category.Alias);
        await Task.WhenAll(new List<Task>()
        {
            UpdateAsync(category, cancellationToken),
            _sequenceCaching.DeleteAsync(key, cancellationToken: cancellationToken)
        });
    }

    public async Task<Guid> DeleteCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        string key = BaseCacheKeys.GetSystemRecordByIdKey(_tableName, category.Alias);
        await Task.WhenAll(new List<Task>()
        {
            DeleteAsync(category, cancellationToken),
            _sequenceCaching.DeleteAsync(key, cancellationToken: cancellationToken)
        });

        return category.Id;
    }

    public async Task DeleteMultipleSupplierAsync(IList<Category> categories,
        CancellationToken cancellationToken = default)
    {
        await Task.WhenAll(new List<Task>()
            {
                DeleteAsync(categories, cancellationToken),
            }
            .Concat(categories.Select(category =>
                _sequenceCaching.DeleteAsync(BaseCacheKeys.GetSystemRecordByIdKey(_tableName, category.Alias),
                    cancellationToken: cancellationToken))));
    }
}