using System.ComponentModel.DataAnnotations.Schema;
using Catalog.Domain.Constants;
using SharedKernel.Domain;

namespace Catalog.Domain.Entities;

[Table(TableName.ProductVariantAttribute)]
public class ProductVariantAttribute : Entity
{
    public string Value { get; set; }
    
    #region Relationships

    public Guid ProductVariantId { get; set; }
    public Guid AttributeId { get; set; }
    
    #endregion
    
    #region Navigations
    
    public virtual ProductVariant ProductVariant { get; set; }
    public virtual Attribute Attribute { get; set; }
    
    #endregion
}