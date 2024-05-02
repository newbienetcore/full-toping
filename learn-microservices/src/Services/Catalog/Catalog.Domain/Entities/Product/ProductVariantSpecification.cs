using System.ComponentModel.DataAnnotations.Schema;
using Catalog.Domain.Constants;
using SharedKernel.Domain;

namespace Catalog.Domain.Entities;

[Table(TableName.ProductVariantSpecification)]
public class ProductVariantSpecification : Entity
{
    public string Key { get; set; }
    public string Value { get; set; }
    
    #region Relationships

    public Guid ProductVariantId { get; set; }

    #endregion
    
    #region Navigations
    
    public virtual ProductVariant ProductVariant { get; set; }
    
    #endregion
}