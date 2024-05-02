using System.ComponentModel.DataAnnotations.Schema;
using Catalog.Domain.Constants;
using SharedKernel.Domain;

namespace Catalog.Domain.Entities;

[Table(TableName.ProductPricing)]
public class ProductPricing : Entity
{
    public decimal SalePrice { get; set; }
    
    public decimal PurchasePrice { get; set; }
    
    public DateTime CreateDate { get; set; }
    
    public DateTime ExpiryDate { get; set; }
    
    public bool InActive { get; set; }
    
    #region Relationships

    public Guid ProductVariantId { get; set; }
    #endregion
    
    #region Navigations
    
    public virtual ProductVariant ProductVariant { get; set; }
    
    #endregion
}