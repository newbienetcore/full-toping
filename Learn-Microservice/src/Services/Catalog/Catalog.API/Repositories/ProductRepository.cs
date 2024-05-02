using Catalog.API.Entities;
using Catalog.API.Persistence;
using Catalog.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using Infrastructure.Common;

namespace Catalog.API.Repositories;

public class ProductRepository : Repository<Product, Guid, ApplicationDbContext>, IProductRepository
{
    public ProductRepository(ApplicationDbContext dbContext, IUnitOfWork<ApplicationDbContext> unitOfWork) : base(dbContext)
    {
        
    }
}