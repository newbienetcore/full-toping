namespace Catalog.Application.Services;

public interface ICachingService
{
    Task<bool> ClearAllCachingAsync(CancellationToken cancellationToken = default);
}