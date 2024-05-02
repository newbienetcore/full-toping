using System.Collections;

namespace Notifications.Domain.Entities;

public class NotificationType
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public Guid GroupId { get; set; }
    public int Status { get; set; }
    
    #region [REFERENCE PROPERTIES]
    public virtual NotificationGroup NotificationGroup { get; set; }
    public ICollection<NotificationTypeChannel> NotificationTypeChannels { get; set; }
    #endregion [REFERENCE PROPERTIES]
}