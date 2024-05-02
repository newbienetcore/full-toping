using Caching;
using Catalog.Application.DTOs;
using Catalog.Application.Repositories;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories;

public class AttributeReadOnlyRepository : EfCoreReadOnlyRepository<Attribute, ApplicationDbContext>, IAttributeReadOnlyRepository
{
    public AttributeReadOnlyRepository(
        ApplicationDbContext dbContext,
        ICurrentUser currentUser,
        ISequenceCaching sequenceCaching,
        IServiceProvider provider
    ) : base(dbContext, currentUser, sequenceCaching, provider)
    {
    }

    public async Task<IList<AttributeDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        string key = BaseCacheKeys.GetSystemFullRecordsKey(_tableName);
        var attributes = await _sequenceCaching.GetAsync<IList<AttributeDto>>(key, cancellationToken: cancellationToken);
        if (attributes != null)
        {
            return attributes;
        }

        attributes = await FindAll()
            .Select(e => new AttributeDto
            {
                Key = e.Key,
                Value = e.Value
            })
            .ToListAsync(cancellationToken);

        if (attributes.Any())
        {
            await _sequenceCaching.SetAsync(key, attributes, cancellationToken: cancellationToken);   
        }

        return attributes;
    }

    public async Task<AttributeDto?> GetAttributeByIdAsync(Guid attributeId, CancellationToken cancellationToken = default)
    {
        string key = BaseCacheKeys.GetSystemRecordByIdKey(_tableName, attributeId);
        
        var entityDto = await _sequenceCaching.GetAsync<AttributeDto>(key, cancellationToken: cancellationToken);

        if (entityDto != null)
        {
            return entityDto;
        }

        entityDto = await _dbSet.Where(e => e.Id == attributeId)
            .Select(e => new AttributeDto()
            {
                Key = e.Key,
                Value = e.Value
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (entityDto != null)
        {
            await _sequenceCaching.SetAsync(key, entityDto, cancellationToken: cancellationToken);   
        }

        return entityDto;
    }

    public async Task<string> IsDuplicate(Guid? attributeId, string key, string value, CancellationToken cancellationToken = default)
    {
        var duplicateSupplier = await _dbSet.FirstOrDefaultAsync(
            e => (attributeId == null || e.Id != attributeId) && (e.Key == key || e.Value == value), cancellationToken);
        
        if (duplicateSupplier is null && attributeId != null)
        {
            return string.Empty;
        }

        if (duplicateSupplier.Key == key)
        {
            return "attribute_is_duplicate_key";
        }

        if (duplicateSupplier.Value == value)
        {
            return "attribute_is_duplicate_value";
        }

        return string.Empty;
    }
}