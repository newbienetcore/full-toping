using Caching;
using Catalog.Application.Services;

namespace Catalog.Infrastructure.Services;

public class CachingService : ICachingService
{
    private readonly ISequenceCaching _caching;
    public CachingService(ISequenceCaching caching)
    {
        _caching = caching;
    }
    public async Task<bool> ClearAllCachingAsync(CancellationToken cancellationToken = default)
    {
        await _caching.DeleteByPatternAsync("*", cancellationToken: cancellationToken);
        return true;
    }
}