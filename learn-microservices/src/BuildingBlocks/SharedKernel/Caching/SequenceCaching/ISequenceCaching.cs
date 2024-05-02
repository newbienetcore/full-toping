namespace Caching;

public interface ISequenceCaching
{
    public TimeSpan DefaultAbsoluteExpireTime { get; }
    
    Task<object?> GetAsync(string key, CachingType type = CachingType.Couple, CancellationToken cancellationToken = default);

    Task<T?> GetAsync<T>(string key, CachingType type = CachingType.Couple, CancellationToken cancellationToken = default);

    Task<string?> GetStringAsync(string key, CachingType type = CachingType.Couple, CancellationToken cancellationToken = default);

    Task SetAsync(string key, object value, TimeSpan? absoluteExpireTime = null,  bool keepTtl = false, CachingType onlyUseType = CachingType.Couple, CancellationToken cancellationToken = default);

    Task DeleteAsync(string key, CachingType type = CachingType.Couple, CancellationToken cancellationToken = default);

    Task DeleteByPatternAsync(string pattern, CachingType type = CachingType.Couple, CancellationToken cancellationToken = default);
}