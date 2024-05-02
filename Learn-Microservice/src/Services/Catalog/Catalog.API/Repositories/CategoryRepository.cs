using Catalog.API.Entities;
using Catalog.API.Persistence;
using Catalog.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using Infrastructure.Common;

namespace Catalog.API.Repositories;

public class CategoryRepository : Repository<Category, Guid, ApplicationDbContext>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext dbContext, IUnitOfWork<ApplicationDbContext> unitOfWork) : base(dbContext)
    {
        
    }
}