namespace Notifications.Domain.Entities;

public class NotificationTypeUser
{
    public Guid Id { get; set; }
    public Guid NotificationTypeId { get; set; }
    public int Channel { get; set; }
    public int Enabled { get; set; }
    public Guid? WorkspaceId { get; set; }
    public string UserId { get; set; }
    
    #region [REFERENCE PROPERTIES]
    public virtual NotificationType NotificationType { get; set; }
    public virtual User User { get; set; }
    // public virtual Workspace Workspace { get; set; }
    #endregion [REFERENCE PROPERTIES]
}