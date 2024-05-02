namespace SharedKernel.Domain;

public interface IDomainEntity
{
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }

    void AddDomainEvent(DomainEvent @event);

    void RemoveDomainEvent(DomainEvent @event);

    void ClearDomainEvents();
}
