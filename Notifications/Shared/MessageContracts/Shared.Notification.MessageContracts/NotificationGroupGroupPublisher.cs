using Shared.MessageBroker.Abstractions;
using Shared.Utilities;

namespace Shared.Notification.MessageContracts;

public class NotificationGroupGroupPublisher : INotificationGroupPublisher
{
    private readonly IMessagePublisher _publishEndpoint;

    public NotificationGroupGroupPublisher(IMessagePublisher publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task Publish(string code,
        string name,
        Guid? parentId,
        string[]? variables,
        string[]? supportedUserLevel,
        Guid appId, 
        CancellationToken cancellationToken = default)
    {
        var message = new PushNotificationGroupMessage(
           code,
           name,
           parentId,
           variables,
           supportedUserLevel,
           appId);
        
        await _publishEndpoint.Publish(message, cancellationToken);
    }
    
}