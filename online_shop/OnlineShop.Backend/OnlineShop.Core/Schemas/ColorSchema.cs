using OnlineShop.Core.Schemas.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Schemas
{
	public class ColorSchema : BaseSchema
	{
        public string Name { get; set; }
		public List<ProductColor> ProductColors { get; set; }
    }
}
