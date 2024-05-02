namespace SharedKernel.Domain;

public abstract class PersonalizedEntityAuditBase : EntityAuditBase, IPersonalizeEntity
{
    public Guid OwnerId { get; set; }
}
