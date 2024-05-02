using System.ComponentModel.DataAnnotations.Schema;
using Catalog.Domain.Constants;
using SharedKernel.Domain;

namespace Catalog.Domain.Entities;

[Table(TableName.ProductWeight)]
public class ProductWeight : Entity
{
    public decimal Value { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    
    #region Relationships
    
    public Guid ProductId { get; set; }
    public Guid WeightCategoryId { get; set; }
    
    #endregion
    
    #region Navigations
    
    public virtual Product Product { get; set; }
    public virtual Weight Weight { get; set; }
    
    #endregion
}