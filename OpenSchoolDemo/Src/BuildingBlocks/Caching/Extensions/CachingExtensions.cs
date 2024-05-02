using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Caching;

public static class CachingExtensions
{
    public static IServiceCollection AddInMemoryCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddSingleton<IMemoryCaching>(s =>
            new MemoryCaching(s.GetRequiredService<IMemoryCache>()));

        return services;
    }

    public static IServiceCollection AddDistributedRedisCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var redisCacheSettings = configuration.GetSection(RedisCacheSetting.SectionName).Get<RedisCacheSetting>();
        var asyncPolicy = PollyExtensions.CreateDefaultPolicy(cfg =>
        {
            cfg.Or<RedisServerException>()
                .Or<RedisConnectionException>();
        });
        
        services.AddSingleton<IDistributedRedisCache>(s => new DistributedRedisCache(
            redisCacheSettings.ConnectionString,
            redisCacheSettings.InstanceName, 
            redisCacheSettings.DatabaseIndex,
            asyncPolicy));

        return services;
    }
    
    public static IServiceCollection AddCoreCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInMemoryCaching(configuration);

        services.AddDistributedRedisCaching(configuration);
        
        services.AddSingleton<ISequenceCaching, SequenceCaching>();
        
        return services;
    }
}