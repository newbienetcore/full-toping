using Notifications.Domain.Abstractions.Common.Interfaces;

namespace Notifications.Domain.Common.Audits;

public abstract class AuditEntityBase : ICreationAuditEntity, IModificationAuditEntity
{
    public DateTime CreatedTime { get; set; }
    public Guid? CreatorId { get; set; }
    public DateTime? ModifiedTime { get; set; }
    public Guid? ModifierId { get; set; }
}