using OnlineShop.Core;
using OnlineShop.Core.Schemas; 
using OnlineShop.Utils;
using OnlineShop.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace OnlineShopS.UseCases.Product.Crud
{
	public class CrudProductFlow
	{
        private readonly IDataContext dbContext;
        public CrudProductFlow(IDataContext ctx)
        {
            dbContext = ctx;
        }
        public Response Create(ProductSchema product)
		{
			var result = dbContext.Products.Add(product);
			return new Response(Message.SUCCESS, result);
		}
		public Response Get(int id)
		{
			var result = dbContext.Products
				.Include(p => p.Brand)
				.FirstOrDefault(p => p.Id == id);
			return new Response(Message.SUCCESS, result);
		}
	}
}
