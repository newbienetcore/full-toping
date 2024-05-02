using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineShop.Core.Schemas.Base;

namespace OnlineShop.Core.Schemas
{
	public class ProductOrderSchema : BaseSchema
	{
		public int ProductID { get; set; }
		public ProductSchema Product { get; set; }
		public int OrderID { get; set; }
		public OrderSchema Order { get; set; }
		public int Quantity { get; set; }
		public float Price { get; set; }
	}
}
