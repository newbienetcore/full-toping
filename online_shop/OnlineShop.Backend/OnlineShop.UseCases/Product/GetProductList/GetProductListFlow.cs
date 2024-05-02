using OnlineShop.Core;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Models;
using OnlineShop.Utils; 

namespace OnlineShop.UseCases.Product.GetProductList
{
	public class GetProductListFlow
	{
		private readonly IDataContext dbContext;
		private readonly IProduct product;
		public GetProductListFlow(IDataContext ctx,IProduct pdt)
		{
            dbContext = ctx;
			product = pdt;
		}

		public Response List()
		{
			var products = dbContext.Products.ToList();
			return new Response(Message.SUCCESS, products);
		}
        public Response List(string name)
        {
            var products = dbContext.Products.Where(u => u.Name.Contains(name)).ToList();
            return new Response(Message.SUCCESS, products);
        }
		public Response GetProducts(ProductStatus? status, int? count) 
		{
            var products = product.GetProducts(status, count);
            return new Response(Message.SUCCESS, products);
        }
    }
}
