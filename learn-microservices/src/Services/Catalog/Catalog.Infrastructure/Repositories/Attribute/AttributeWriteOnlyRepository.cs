using Caching;
using Catalog.Application.Repositories;
using Catalog.Infrastructure.Persistence;

namespace Catalog.Infrastructure.Repositories;

public class AttributeWriteOnlyRepository : 
    EfCoreWriteOnlyRepository<Attribute, ApplicationDbContext>, IAttributeWriteOnlyRepository
{
    public AttributeWriteOnlyRepository(
        ApplicationDbContext dbContext, 
        ICurrentUser currentUser,
        ISequenceCaching sequenceCaching, 
        IServiceProvider provider
        ) : base(dbContext, currentUser, sequenceCaching, provider)
    {
    }
    
}