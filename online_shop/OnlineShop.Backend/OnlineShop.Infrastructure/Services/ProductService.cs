using OnlineShop.Core.Schemas;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Models;

namespace OnlineShop.Infrastructure.Services
{

	public class ProductService : IProduct
	{

		private readonly IDataContext context;

		public ProductService(IDataContext _ctx)
		{
			this.context = _ctx;
		}

		public ProductSchema Get(int productId)
		{
			ProductSchema product = (
			  from p in context.Products
			  where p.Id == productId
			  select p
			).FirstOrDefault();

			return product!;
		}

        public List<ProductSchema> GetProducts(ProductStatus? status = null, int? quantity = null)
        {
            var query = context.Products.AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value);
            }

            if (quantity.HasValue)
            {
                query = query.OrderByDescending(p => p.CreatedAt).Take(quantity.Value);
            }

            var products = query.ToList();
            return products;
        }

    }
}
