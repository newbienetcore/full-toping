using Catalog.Application.DTOs;
using Catalog.Application.Persistence;
using SharedKernel.Application.Repositories;

namespace Catalog.Application.Repositories;

public interface IAttributeWriteOnlyRepository : IEfCoreWriteOnlyRepository<Attribute, IApplicationDbContext>
{
    
}