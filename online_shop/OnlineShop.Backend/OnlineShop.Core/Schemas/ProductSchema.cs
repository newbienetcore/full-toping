

using OnlineShop.Core.Models;
using OnlineShop.Core.Schemas.Base;

namespace OnlineShop.Core.Schemas
{
    public class ProductSchema : BaseSchema
    {

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public CategorySchema Category { get; set; }
        public int BrandID { get; set; }
        public BrandSchema Brand { get; set; }
        public float Price { get; set; }
        public float Discount { get; set; }

		public string Currentcy { get; set; }
        public string? DefaultImage { get; set; }
        public List<ImageSchema> Images { get; set; }
        public string OriginLinkDetail { get; set; }
        public string Url { get; set; }
        public int Stock { get; set; }
        public ProductStatus Status { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
		public List<ProductColor> ProductColors { get; set; }

		// public ICollection<ProductOrderSchema> ProductOrders { get; set; }
	}
}
