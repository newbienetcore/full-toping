namespace Notifications.Domain.Entities;

public class NotificationTypeChannelConfig
{
    public Guid Id { get; set; }
    public Guid NotificationTypeChannelId { get; set; }
    public Guid WorkspaceId { get; set; }
    public int Enabled { get; set; }
    
    #region [REFERENCE PROPERTIES]
    public virtual NotificationTypeChannel NotificationTypeChannel { get; set; }
    // public virtual Workspace Workspace { get; set; }
    #endregion [REFERENCE PROPERTIES]
}