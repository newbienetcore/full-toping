namespace SharedKernel.Domain;

public interface IEntityAuditBase<TKey> :  IAuditable, IEntityBase<TKey>
{

}

public interface IEntityAuditBase : IEntityAuditBase<Guid>
{
    
}