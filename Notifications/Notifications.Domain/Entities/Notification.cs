using Notifications.Domain.Enums;

namespace Notifications.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public Guid NotificationTypeId { get; set; }
    public int ChannelId { get; set; }
    public Guid AppId { get; set; }
    public Guid NotificationGroupId { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public ENotificationStatus Status { get; set; }
    
    #region [REFERENCE PROPERTIES]
    public virtual User User { get; set; }
    public virtual NotificationType NotificationType { get; set; }
    // public virtual Channel Channel { get; set; }
    public virtual App App { get; set; }
    public virtual NotificationGroup NotificationGroup { get; set; }
    #endregion [REFERENCE PROPERTIES]
}