namespace SharedKernel.Domain;

public interface IEntityBase<T> : ICoreEntity, ICloneable
{
    T Id { get; set; }
}

public interface IEntityBase : IEntityBase<Guid>
{
    
}