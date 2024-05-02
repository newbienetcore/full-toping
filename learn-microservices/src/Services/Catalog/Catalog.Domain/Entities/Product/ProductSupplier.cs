using System.ComponentModel.DataAnnotations.Schema;
using Catalog.Domain.Constants;
using SharedKernel.Domain;

namespace Catalog.Domain.Entities;

[Table(TableName.ProductSupplier)]
public class ProductSupplier : Entity
{
    #region Relationships

    public Guid ProductId { get; set; }
    public Guid SupplierId { get; set; }

    #endregion
    
    #region Navigations
    
    public virtual Product Product { get; set; }
    public virtual Supplier Supplier { get; set; }
    
    #endregion
}