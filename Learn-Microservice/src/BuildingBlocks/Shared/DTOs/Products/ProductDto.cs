using FluentValidation;

namespace Shared.DTOs.Products;

public class ProductDto
{
    public Guid Id { get; set; }
    public string No { get; set; }
    public string Name { get; set; }
    public string Summary { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }
    public List<string>? ProductImages { get; set; }
}
