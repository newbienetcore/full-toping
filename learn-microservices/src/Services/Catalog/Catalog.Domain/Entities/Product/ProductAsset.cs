using System.ComponentModel.DataAnnotations.Schema;
using Catalog.Domain.Constants;
using Catalog.Domain.Enum;
using SharedKernel.Domain;

namespace Catalog.Domain.Entities;

[Table(TableName.ProductAsset)]
public class ProductAsset : Entity
{
    public ProductImageType ImageType { get; set; }
    
    #region Relationships

    public Guid ProductId { get; set; }
    public Guid AssetId { get; set; }

    public Guid ProductVariantId { get; set; }
    #endregion
    
    #region Navigations
    
    public virtual Product Product { get; set; }
    public virtual Asset Asset { get; set; }
    public virtual ProductVariant ProductVariant { get; set; }
    
    #endregion
}
