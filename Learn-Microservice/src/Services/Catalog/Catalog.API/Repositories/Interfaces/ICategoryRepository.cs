using Catalog.API.Entities;
using Catalog.API.Persistence;
using Contracts.Common.Interfaces;

namespace Catalog.API.Repositories.Interfaces;

public interface ICategoryRepository : IRepository<Category, Guid, ApplicationDbContext>
{
    
}