using System.ComponentModel.DataAnnotations.Schema;
using Catalog.Domain.Constants;
using SharedKernel.Domain;

namespace Catalog.Domain.Entities;

[Table(TableName.ProductCategory)]
public class ProductCategory : Entity
{
    #region Relationships

    public Guid ProductId { get; set; }
    public Guid CategoryId { get; set; }

    #endregion
    
    #region Navigations
    
    public virtual Product Product { get; set; }
    public virtual Category Category { get; set; }
    
    #endregion
}