using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Caching.Abstractions;
using Shared.Utilities;
using StackExchange.Redis;

namespace Shared.Caching.Redis.Extensions;

public static class CachingExtensions
{
    public static IServiceCollection AddRedisCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var cacheProvider = configuration.GetOptions<CachingOptions>("Redis");
        var asyncPolicy = PollyExtensions.CreateDefaultPolicy(cfg =>
        {
            cfg.Or<RedisServerException>()
                .Or<RedisConnectionException>();
        });

        services.AddSingleton<ICacheService>(s => new CacheService(
            cacheProvider.ConnectionString,
            cacheProvider.InstanceName, 
            cacheProvider.DatabaseIndex,
            asyncPolicy));

        return services;
    }
}