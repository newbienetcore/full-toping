using Catalog.API.Entities;
using Catalog.API.Persistence;
using Contracts.Common.Interfaces;

namespace Catalog.API.Repositories.Interfaces;

public interface IProductRepository : IRepository<Product, Guid, ApplicationDbContext>
{
    
}