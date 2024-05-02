namespace Notifications.Domain.Events.NotificationGroups;

public class NotificationGroupCreatedEvent
{
    public NotificationGroup Item { get; }
    
    public NotificationGroupCreatedEvent(NotificationGroup item)
    {
        Item = item;
    }
    
}