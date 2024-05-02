using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using EventBus.Messages.IntegrationEvents.Interfaces;
using Infrastructure.Common;
using Infrastructure.Configurations;
using Infrastructure.Extensions;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Configurations;

namespace Basket.API;

public static class ConfigureServices
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var eventBusSettings = configuration.GetOptions<EventBusSettings>(nameof(EventBusSettings));
        if (eventBusSettings is null) throw new ArgumentNullException(nameof(eventBusSettings));
        services.AddSingleton(eventBusSettings);
        
        var cacheSettings = configuration.GetOptions<CacheSettings>(nameof(CacheSettings));
        if (cacheSettings is null) throw new ArgumentNullException(nameof(cacheSettings));
        services.AddSingleton(cacheSettings);
        
        return services;
    }
    
    public static IServiceCollection AddServices(this IServiceCollection services)
        => services.AddScoped<IBasketRepository, BasketRepository>()
            .AddTransient<ISerializeService, SerializeService>();

    public static void AddRedisServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Redis Configuration
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetOptions<CacheSettings>(CacheSettings.SectionName)?.ConnectionString 
                                    ?? throw new ArgumentNullException(nameof(CacheSettings));
        });
    }

    public static IServiceCollection ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        var eventBusSettings = configuration.GetOptions<EventBusSettings>(nameof(EventBusSettings));
        if (eventBusSettings is null) throw new ArgumentNullException(nameof(eventBusSettings));

        var mqConnection = new Uri(eventBusSettings.HostAddress);
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(mqConnection);
            });
            
            // Publish submit order message
            config.AddRequestClient<IBasketCheckoutEvent>();
        });
        
        services.AddMassTransitHostedService();

        return services;
    }

    public static IServiceCollection AddConfigurationSetting(this IServiceCollection services,
        IConfiguration configuration)
    {
        var eventBusSettings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();
        services.AddSingleton(eventBusSettings);
        
        return services;
    }
}