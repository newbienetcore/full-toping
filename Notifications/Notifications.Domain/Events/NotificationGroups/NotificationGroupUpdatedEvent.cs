namespace Notifications.Domain.Events.NotificationGroups;

public class NotificationGroupUpdatedEvent
{
    public NotificationGroup Item { get; }
    
    public NotificationGroupUpdatedEvent(NotificationGroup item)
    {
        Item = item;
    }
}