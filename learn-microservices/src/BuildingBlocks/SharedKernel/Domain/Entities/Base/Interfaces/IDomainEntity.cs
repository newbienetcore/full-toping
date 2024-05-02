namespace SharedKernel.Domain
{
    public interface IDomainEntity<TKey> : IBaseEntity<TKey>
    {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }

        void AddDomainEvent(DomainEvent @event);

        void RemoveDomainEvent(DomainEvent @event);

        void ClearDomainEvents();
    }

    public interface IDomainEntity : IDomainEntity<Guid> { }
}