using System.ComponentModel.DataAnnotations.Schema;
using Catalog.Domain.Constants;
using Catalog.Domain.Enum;
using SharedKernel.Domain;

namespace Catalog.Domain.Entities;

[Table(TableName.CategoryDiscount)]
public class CategoryDiscount : BaseEntity
{
    public float DiscountValue { get; set; }
    public DiscountUnit DiscountUnit { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
    public string CouponCode { get; set; }
    public int MinimumOrderValue { get; set; }
    public int MaximumDiscountAmount { get; set; }
    public bool IsRedeemAllowed { get; set; }
    
    #region Relationships
    
    public Guid CategoryId { get; set; }

    #endregion
    
    #region Navigations
    
    public virtual Category Category { get; set; }
    
    #endregion
}