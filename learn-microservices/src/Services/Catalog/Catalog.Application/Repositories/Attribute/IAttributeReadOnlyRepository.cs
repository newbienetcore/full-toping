using Catalog.Application.DTOs;
using Catalog.Application.Persistence;
using SharedKernel.Application.Repositories;

namespace Catalog.Application.Repositories;

public interface IAttributeReadOnlyRepository : IEfCoreReadOnlyRepository<Attribute, IApplicationDbContext>
{
    Task<IList<AttributeDto>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<AttributeDto?> GetAttributeByIdAsync(Guid attributeId, CancellationToken cancellationToken = default);
    
    Task<string> IsDuplicate(Guid? attributeId, string key, string value, CancellationToken cancellationToken = default);
}