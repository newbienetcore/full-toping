namespace Notifications.Domain.Abstractions.Common.Interfaces;

public interface IModificationAuditEntity
{
    DateTime? ModifiedTime { get; set; }
    Guid? ModifierId { get; set; }
}