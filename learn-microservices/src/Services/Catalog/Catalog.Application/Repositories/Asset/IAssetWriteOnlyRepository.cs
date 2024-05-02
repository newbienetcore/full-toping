using Catalog.Application.Persistence;
using Catalog.Domain.Entities;
using SharedKernel.Application.Repositories;

namespace Catalog.Application.Repositories;

public interface IAssetWriteOnlyRepository : IEfCoreWriteOnlyRepository<Asset, IApplicationDbContext>
{
    
}