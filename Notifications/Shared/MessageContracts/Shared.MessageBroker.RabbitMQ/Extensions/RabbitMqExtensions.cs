using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.MessageBroker.Abstractions;
using Shared.Utilities;

namespace Shared.MessageBroker.RabbitMQ.Extensions;

public static class RabbitMqExtensions
{
    public static IServiceCollection AddMasstransitRabbitMqMessagePublisher(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator, MessageQueueSetting> registerConsumer = null,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator, MessageQueueSetting> configConsumer = null)
    {
        var setting = configuration.GetOptions<MessageQueueSetting>(MessageQueueSetting.SettingSection);
        services.AddMassTransit(configurator =>
        {
            registerConsumer?.Invoke(configurator, setting);
            
            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(setting.Host, setting.Port, setting.VirtualHost, h =>
                {
                    h.Username(setting.UserName);
                    h.Password(setting.Password);
                });
                
                configConsumer?.Invoke(context, cfg, setting);
            });
        });

        services.AddTransient<IMessagePublisher, MasstransitMessagePublisher>();
        
        return services;
    }

    // public static IServiceCollection AddMasstransitInMemoryMessagePublisher(
    //     this IServiceCollection services,
    //     IConfiguration configuration, 
    //     Action<IBusRegistrationConfigurator>? registerConsumer = null,
    //     Action<IBusRegistrationContext, IInMemoryBusFactoryConfigurator>? configConsumer = null)
    // {
    //
    //     services.AddMassTransit(configurator =>
    //     {
    //         registerConsumer?.Invoke(configurator);
    //         configurator.UsingInMemory((context, cfg) =>
    //         {
    //             if (configConsumer is not null)
    //                 configConsumer(context, cfg);
    //             else
    //                 configurator.ConfigureEndpoints(context);
    //         });
    //     });
    //     
    //     services.AddTransient<IMessagePublisher, MasstransitMessagePublisher>();
    //     return services;
    // }
}