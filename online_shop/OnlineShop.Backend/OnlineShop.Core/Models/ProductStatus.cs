using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Models
{
    [Flags]
    public enum ProductStatus
    {
        None = 0,
        FreeShip = 1,
        Trend = 2,
        Featured = 4,
        BestSeller = 8,
    }
}
