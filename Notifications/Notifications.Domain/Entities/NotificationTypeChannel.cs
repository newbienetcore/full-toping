using System.Collections;

namespace Notifications.Domain.Entities;

public class NotificationTypeChannel
{
    public Guid Id { get; set; }
    public Guid NotificationTypeId { get; set; }
    public int Channel { get; set; }
    public int Enabled { get; set; }
    
    #region [REFERENCE PROPERTIES]
    public virtual NotificationType NotificationType { get; set; }
    public ICollection<NotificationTypeChannelTemplate> NotificationTypeChannelTemplates { get; set; }
    public ICollection<NotificationTypeChannelConfig> NotificationTypeChannelConfigs { get; set; }
    #endregion [REFERENCE PROPERTIES]
}