using OnlineShop.Core.Schemas.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Schemas
{
	public class ProductColor : BaseSchema
	{
        public int ProductID { get; set; }
        public ProductSchema Product { get; set; }
        public int ColorID { get; set; }
        public ColorSchema Color { get; set; }
    }
}
