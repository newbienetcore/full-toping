using Infrastructure.Configurations;
using Infrastructure.Extensions;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.API.Application.IntegrationEvents.EventsHandler;
using Shared.Configurations;

namespace Ordering.API;

public static class ConfigureServices
{
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var emailSettings = configuration.GetOptions<EmailSMTPSettings>(EmailSMTPSettings.Section);
        if (emailSettings is null) throw new ArgumentNullException(nameof(emailSettings));
        services.AddSingleton(emailSettings);

        var eventBusSettings = configuration.GetOptions<EventBusSettings>(EventBusSettings.Section);
        if (eventBusSettings is null || string.IsNullOrEmpty(eventBusSettings.HostAddress)) throw new ArgumentNullException(nameof(eventBusSettings));
        services.AddSingleton(eventBusSettings);
        
        return services;
    }

    public static void ConfigureMasstransit(this IServiceCollection services, IConfiguration configuration)
    {
        var eventBusSettings = configuration.GetOptions<EventBusSettings>(EventBusSettings.Section);
        if (eventBusSettings is null || string.IsNullOrEmpty(eventBusSettings.HostAddress)) throw new ArgumentNullException(nameof(eventBusSettings));

        var mqConnection = new Uri(eventBusSettings.HostAddress);
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(config =>
        {
            config.AddConsumersFromNamespaceContaining<BasketCheckoutEventHandler>();
            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(mqConnection);
                cfg.ConfigureEndpoints(ctx);
            });
        });
        
        services.AddMassTransitHostedService();
        
    }
}