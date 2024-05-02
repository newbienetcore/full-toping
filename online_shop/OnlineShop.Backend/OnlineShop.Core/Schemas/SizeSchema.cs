using OnlineShop.Core.Schemas.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Schemas
{
	public class SizeSchema : BaseSchema
	{
        public string Name { get; set; }
		public List<ProductSize> ProductSizes { get; set; }
    }
}
