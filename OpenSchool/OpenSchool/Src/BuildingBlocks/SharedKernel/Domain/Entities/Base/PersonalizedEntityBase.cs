namespace SharedKernel.Domain;

public abstract class PersonalizedEntityBase : EntityBase, IPersonalizeEntity
{
    public Guid OwnerId { get; set; }
}
