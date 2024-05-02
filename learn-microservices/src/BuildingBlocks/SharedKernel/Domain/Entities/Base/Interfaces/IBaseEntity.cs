namespace SharedKernel.Domain
{
    public interface IBaseEntity<TKey> : IEntity<TKey> ,ICoreEntity, IAuditable, ICloneable
    {
        
    }

    public interface IBaseEntity : IBaseEntity<Guid> { }
}
