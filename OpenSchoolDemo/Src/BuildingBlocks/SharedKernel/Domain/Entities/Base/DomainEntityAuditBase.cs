using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.Domain;

public class DomainEntityAuditBase<TKey> : EntityAuditBase<TKey>, IDomainEntity
{
    private List<DomainEvent> _domainEvents;

    [NotMapped, Libraries.Ignore]
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

    public void AddDomainEvent(DomainEvent @event)
    {
        _domainEvents = _domainEvents ?? new List<DomainEvent>();
        _domainEvents.Add(@event);
    }

    public void RemoveDomainEvent(DomainEvent @event)
    {
        _domainEvents?.Remove(@event);
    }

    public void ClearDomainEvents()
    {
        _domainEvents = null;
    }
}
    
public class DomainEntityAuditBase : DomainEntityAuditBase<Guid> 
{
   
}