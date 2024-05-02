using OnlineShop.Core;
using OnlineShop.Core.Models;
using OnlineShop.Core.Schemas; 
namespace OnlineShop.Core.Interfaces
{
	public interface IProduct
	{
		ProductSchema Get(int productId);
		List<ProductSchema> GetProducts(ProductStatus? status, int? count);

    } 
}
