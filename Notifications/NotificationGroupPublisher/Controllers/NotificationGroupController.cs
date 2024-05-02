using Microsoft.AspNetCore.Mvc;
using Shared.MessageBroker.Abstractions;
using Shared.Notification.MessageContracts;

namespace NotificationGroupPublisher.Controllers;

public class NotificationGroupController : ApiControllerBase
{
    private readonly IMessagePublisher _messagePublisher;
    
    public NotificationGroupController(IMessagePublisher messagePublisher)
    {
        _messagePublisher = messagePublisher;
    }

    [HttpPost]
    [Route("api/notification--group-publisher")]
    public async Task<IActionResult> PublisherAsync([FromBody] PushNotificationGroupMessage publisher, CancellationToken cancellationToken = default)
    {
        await _messagePublisher.Publish(publisher, cancellationToken);
        return Content("Publisher notification group successful.");
    }

}