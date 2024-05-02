namespace NotificationGroupPublisher.Models;

public class NotificationGroupPublisher
{
    public string Code { get; set; }
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public string[]? Variables { get; set; }
    public string[]? SupportedUserLevel { get; set; }
    
    public Guid AppId { get; set; }
}

