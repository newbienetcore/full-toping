using System.ComponentModel.DataAnnotations.Schema;
using Catalog.Domain.Constants;
using SharedKernel.Domain;

namespace Catalog.Domain.Entities;

[Table(TableName.ProductReview)]
public class ProductReview : BaseEntity
{
    public float Rating { get; set; }
    public string Comment { get; set; }
    public Guid UserId { get; set; }
    public string UserInfo { get; set; }
    
    #region Relationships

    public Guid ProductId { get; set; }

    #endregion
    
    #region Navigations
    
    public virtual Product Product { get; set; }
    
    #endregion
}