using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SharedKernel.Domain
{
    public abstract class BaseEntity<TKey> : CoreEntity, IBaseEntity<TKey>
    {
        [System.ComponentModel.DataAnnotations.Key]
        public TKey Id { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; }
        
        public DateTime CreatedDate { get; set; } = SharedKernel.Libraries.DateHelper.Now;

        public Guid CreatedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public Guid? LastModifiedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public Guid? DeletedBy { get; set; }

        #region Cloneable
        public object Clone()
        {
            return MemberwiseClone();
        }
        #endregion
    }

    /// <summary>
    /// By default, TKey is long
    /// </summary>
    public class BaseEntity : BaseEntity<Guid>, IBaseEntity
    {
    }
}
