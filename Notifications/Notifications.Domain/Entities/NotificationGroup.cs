using System.Collections;
using Notifications.Domain.Common.Events;

namespace Notifications.Domain.Entities;

public class NotificationGroup
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public string[]? Variables { get; set; }
    public string[]? SupportedUserLevel { get; set; }
    public Guid AppId { get; set; }
    
    #region [REFERENCE PROPERTIES]
    public virtual App App { get; set; }
    public NotificationGroup Parent { get; set; }
    public ICollection<NotificationGroup> Children { get; set; }
    public ICollection<Notification> Notifications { get; set; }
    public ICollection<NotificationType> NotificationTypes { get; set; }
    #endregion [REFERENCE PROPERTIES]
}