using Notifications.Domain.Abstractions.Common.Interfaces;

namespace Notifications.Domain.Common.Audits;

public abstract class FullAuditEntity : ICreationAuditEntity, IModificationAuditEntity, IDeletionAuditEntity
{
    public DateTime CreatedTime { get; set; }
    public Guid? CreatorId { get; set; }
    public DateTime? ModifiedTime { get; set; }
    public Guid? ModifierId { get; set; }
    public DateTime? DeletedTime { get; set; }
    public bool Deleted { get; set; }
}