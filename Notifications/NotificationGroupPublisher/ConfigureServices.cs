using System.Reflection;
using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using MediatR;
using Notifications.WebApi.Consumers;
using RabbitMQ.Client;
using Shared.Crawler.MessageContacts;
using Shared.MessageBroker.RabbitMQ.Extensions;
using Shared.Notification.MessageContracts;
using NotificationTopic = Shared.Notification.MessageContracts.Constant;


namespace NotificationGroupPublisher;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddHttpContextAccessor();
        
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddMessageQueue(configuration, env);

        return services;
    }

    public static IServiceCollection AddMessageQueue(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment env)
    {
        services.AddMasstransitRabbitMqMessagePublisher(
            configuration,
            (configurator, setting) =>
            {
                configurator.AddRequestClient<PushNotificationGroupMessage>(new Uri(
                    setting.GetPublishEndpoint(NotificationTopic.TopicConstant.PushNotificationGroup)));
                
            },
            (context, cfg, setting) =>
            {

                cfg.ConfigureEndpoints(context);
            });
        
        services.AddMassTransitHostedService();
        return services;
    }
}