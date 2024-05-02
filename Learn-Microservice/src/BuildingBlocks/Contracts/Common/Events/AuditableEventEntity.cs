using Contracts.Domains.Interfaces;

namespace Contracts.Common.Events;

public abstract class AuditableEventEntity<T> : EventEntity<T>, IAuditable
{
    public DateTime CreateDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
}