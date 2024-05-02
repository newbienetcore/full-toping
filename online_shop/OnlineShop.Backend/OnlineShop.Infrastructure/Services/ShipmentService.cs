using OnlineShop.Core;
using OnlineShop.Core.Schemas;
using OnlineShop.Core.Interfaces;

namespace OnlineShop.Infrastructure.Services
{
  public class ShipmentService :   IShipment
  {

    private readonly DataContext context;

    public ShipmentService(DataContext _ctx)  
    {
      this.context = _ctx;
    }

  }
}
