namespace Notifications.Domain.Abstractions.Common.Interfaces;

public interface ICreationAuditEntity
{
    DateTime CreatedTime { get; set; }
    Guid? CreatorId { get; set; }
}