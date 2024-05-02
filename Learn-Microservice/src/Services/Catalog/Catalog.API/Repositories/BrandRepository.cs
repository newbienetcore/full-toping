using Catalog.API.Entities;
using Catalog.API.Persistence;
using Catalog.API.Repositories.Interfaces;
using Infrastructure.Common;


namespace Catalog.API.Repositories;

public class BrandRepository : Repository<Brand, Guid, ApplicationDbContext>, IBrandRepository
{
    public BrandRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
    
}