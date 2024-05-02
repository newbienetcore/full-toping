using MassTransit;
using MediatR;
using Notifications.Application.NotificationGroups.Commands.Create;
using Shared.Notification.MessageContracts;

namespace Notifications.WebApi.Consumers;

public class PushNotificationGroupConsumer : IConsumer<PushNotificationGroupMessage>
{
    private readonly IMediator _mediator;
    
    private readonly ILogger<PushNotificationGroupConsumer> _logger;

    public PushNotificationGroupConsumer(IMediator mediator, ILogger<PushNotificationGroupConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PushNotificationGroupMessage> context)
    {
        var message = context.Message;
        var id=  await _mediator.Send(new CreateNotificationGroupCommand()
        {
            Code = message.Code, 
            Name = message.Name,
            ParentId = message.ParentId, 
            Variables = message.Variables, 
            SupportedUserLevel = message.SupportedUserLevel,
            AppId = message.AppId, 
        });
        
        _logger.LogInformation($"~ ~ ~ ~ ~ ~ {DateTime.Now} : Push notification group successful : {id} ~ ~ ~ ~ ~ ~ ~");
    }
}