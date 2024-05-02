namespace Notifications.Domain.Events.NotificationGroups;

public class NotificationGroupDeletedEvent
{
    public NotificationGroup Item { get; }
    
    public NotificationGroupDeletedEvent(NotificationGroup item)
    {
        Item = item;
    }
}