using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SharedKernel.Libraries;

namespace SharedKernel.Domain;

public abstract class EntityAuditBase<TKey> : EntityBase<TKey>, IEntityAuditBase<TKey>
{
    [JsonIgnore]
    public bool IsDeleted { get; set; }
        
    public DateTime CreatedDate { get; set; } = DateHelper.Now;

    public Guid CreatedBy { get; set; }

    public DateTime? LastModifiedDate { get; set; }

    public Guid? LastModifiedBy { get; set; }

    public DateTime? DeletedDate { get; set; }

    public Guid? DeletedBy { get; set; }
}

/// <summary>
/// By default, TKey is long
/// </summary>
public class EntityAuditBase : EntityAuditBase<Guid>, IEntityAuditBase
{
}