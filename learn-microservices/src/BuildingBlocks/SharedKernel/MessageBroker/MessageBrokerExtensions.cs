using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBroker;

public static class MessageBrokerExtensions
{
    public static IServiceCollection AddCoreMessageBroker(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator, RabbitMqSetting> registerConsumer = null,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator, RabbitMqSetting> configConsumer = null)
    {
        var messageQueueSettings = configuration.GetSection(RabbitMqSetting.SectionName)
            .Get<RabbitMqSetting>();

        if (messageQueueSettings is null)
        {
            throw new ArgumentNullException(nameof(RabbitMqSetting));
        }

        services.AddMassTransit(configurator =>
        {
            registerConsumer?.Invoke(configurator, messageQueueSettings);

            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(messageQueueSettings.Host, messageQueueSettings.Port, messageQueueSettings.VirtualHost, h =>
                {
                    h.Username(messageQueueSettings.UserName);
                    h.Password(messageQueueSettings.Password);
                });

                configConsumer?.Invoke(context, cfg, messageQueueSettings);
            });
        });
        
        services.AddMassTransitHostedService();

        services.AddTransient<IMessagePublisher, MasstransitMessagePublisher>();

        return services;
    }
}