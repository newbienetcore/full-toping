using Contracts.Domains;

namespace Catalog.API.Entities;

public class Product : EntityAuditBase<Guid>
{
    public string No { get; set; }
    public string Name { get; set; }
    public string Summary { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; } = 0;
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }

    #region [REFERENCE PROPERTIES]
    
    public virtual Category Category { get; set; }
    public virtual Brand Brand { get; set; }
    public ICollection<ProductImage> ProductImages { get; set; }
    
    #endregion
    
}