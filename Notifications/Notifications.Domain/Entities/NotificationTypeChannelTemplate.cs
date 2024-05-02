namespace Notifications.Domain.Entities;

public class NotificationTypeChannelTemplate
{
    public Guid Id { get; set; }
    public Guid NotificationTypeChannelId {get; set; }
    public Guid LanguageId { get; set; }
    public Guid TemplateId { get; set; }
    public Guid WorkspaceId { get; set; }
    
    #region [REFERENCE PROPERTIES]
    public virtual NotificationTypeChannel NotificationTypeChannel { get; set; }
    // public virtual Language Language { get; set; }
    public virtual ChannelTemplate ChannelTemplate { get; set; }
    // public virtual Workspace Workspace { get; set; }
    #endregion [REFERENCE PROPERTIES]
    
}