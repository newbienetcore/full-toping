using SharedKernel.Domain;
using SharedKernel.Libraries;

namespace SignalR;

public class Notification : PersonalizedEntityAuditBase
{
    public NotificationType Type { get; set; }
    
    public bool IsUnread { get; set; } = true;

    public string Description { get; set; }

    public DateTime Timestamp { get; set; }
}