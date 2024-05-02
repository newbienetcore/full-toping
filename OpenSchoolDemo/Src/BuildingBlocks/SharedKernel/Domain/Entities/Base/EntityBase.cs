namespace SharedKernel.Domain;

public class EntityBase<TKey> : CoreEntity, IEntityBase<TKey>
{
    [System.ComponentModel.DataAnnotations.Key]
    public TKey Id { get; set; }
    
    #region Cloneable
    public object Clone()
    {
        return MemberwiseClone();
    }
    #endregion
}

public class EntityBase : EntityBase<Guid>
{
    
}