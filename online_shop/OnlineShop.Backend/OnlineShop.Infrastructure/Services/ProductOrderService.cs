using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Schemas;

namespace OnlineShop.Infrastructure.Services
{
  public class ProductOrderService : IProductOrder
  {

    private readonly DataContext context;

    public ProductOrderService(DataContext _ctx)
    {
      this.context = _ctx;
    }

  }
}
