using Notifications.Application.Common.Mappings;
using Notifications.Domain.Entities;

namespace Notifications.Application.Products.Models;

public class ProductDto : IMapFrom<Product>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public decimal Price { get; set; }
}