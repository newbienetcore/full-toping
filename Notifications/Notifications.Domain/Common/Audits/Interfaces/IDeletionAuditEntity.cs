namespace Notifications.Domain.Abstractions.Common.Interfaces;

public interface IDeletionAuditEntity
{
    DateTime? DeletedTime { get; set; }
    bool Deleted { get; set; }
}