using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Caching;

public static class CachingExtensions
{
    public static IServiceCollection AddCoreCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var redisCacheSettings = configuration.GetSection(RedisCacheSetting.SectionName).Get<RedisCacheSetting>();
        var asyncPolicy = PollyExtensions.CreateDefaultPolicy(cfg =>
        {
            cfg.Or<RedisServerException>()
                .Or<RedisConnectionException>();
        });
        
        services.AddSingleton<IRedisCache>(s => new RedisCache(
            redisCacheSettings.ConnectionString,
            redisCacheSettings.InstanceName, 
            redisCacheSettings.DatabaseIndex,
            asyncPolicy));
        
        services.AddMemoryCache();
        services.AddSingleton<IMemoryCaching>(s =>
            new MemoryCaching(s.GetRequiredService<IMemoryCache>()));
        
        services.AddSingleton<ISequenceCaching, SequenceCaching>();
        
        return services;
    }
}