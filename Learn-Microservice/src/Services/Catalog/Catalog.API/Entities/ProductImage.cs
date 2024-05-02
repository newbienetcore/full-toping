using Contracts.Domains;

namespace Catalog.API.Entities;

public class ProductImage : EntityBase<Guid>
{
    public string ImageUrl { get; set; }
    public string? Description { get; set; }
    public Guid ProductId { get; set; }
    
    #region [REFERENCE PROPERTIES]
    
    public virtual Product Product { get; set; }

    #endregion
}