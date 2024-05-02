using OnlineShop.Core.Schemas.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Schemas
{
	public class ImageSchema : BaseSchema
	{
		public string Path { get; set; }
        public string Description { get; set; }
		public int ProductID { get; set; }
		public ProductSchema Product { get; set; }
	}
}
