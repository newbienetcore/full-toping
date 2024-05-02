using System.ComponentModel.DataAnnotations.Schema;
using Catalog.Domain.Constants;
using Catalog.Domain.Enum;
using SharedKernel.Domain;

namespace Catalog.Domain.Entities;

[Table(TableName.ProductDiscount)]
public class ProductDiscount : BaseEntity
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
    
    public Guid ProductId { get; set; }

    #endregion
    
    #region Navigations
    
    public virtual Product Product { get; set; }
    
    #endregion
}