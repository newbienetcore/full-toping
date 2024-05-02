using Notifications.Domain.Common.Events;

namespace Notifications.Domain.Events.NotificationGroups;

public class NotificationGroupCompletedEvent : DomainEvent
{
    public NotificationGroup Item { get; }
    public NotificationGroupCompletedEvent(NotificationGroup item)
    {
        Item = item;
    }
}