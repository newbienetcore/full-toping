using Shared.DTOs.Brands;
using Shared.DTOs.Categories;

namespace Shared.DTOs.Products;

public class ProductDetailDto
{
    public string No { get; set; }
    public string Name { get; set; }
    public string Summary { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; } = 0;
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }
    public CategoryDto Category { get; set; }
    public BrandDto Brand { get; set; }
    public ICollection<ProductImageDto> ProductImages { get; set; }
}