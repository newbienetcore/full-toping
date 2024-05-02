using OnlineShop.Core.Schemas.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Schemas
{
	public class ProductSize : BaseSchema
	{
        public int ProductID { get; set; }
        public ProductSchema Product { get; set; }
        public int SizeID { get; set; }
        public SizeSchema Size { get; set; }
    }
}
