using Contracts.Domains.Interfaces;

namespace Contracts.Domains;

public abstract class EntityAuditBase<T> : EntityBase<T>, IAuditable
{
    public DateTime CreateDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
}