using Notifications.Domain.Abstractions.Common.Interfaces;

namespace Notifications.Domain.Common.Audits;

public abstract class AuditEntity : ICreationAuditEntity, IDeletionAuditEntity
{
    public DateTime CreatedTime { get; set; }
    public Guid? CreatorId { get; set; }
    public DateTime? DeletedTime { get; set; }
    public bool Deleted { get; set; }
}