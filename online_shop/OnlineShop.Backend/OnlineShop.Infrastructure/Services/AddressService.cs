using OnlineShop.Core.Interfaces; 
namespace OnlineShop.Infrastructure.Services
{

  public class AddressService : IAddress
  {

    private readonly DataContext context;
    public AddressService(DataContext _ctx)
    {
      context = _ctx;
    }
  }
}
