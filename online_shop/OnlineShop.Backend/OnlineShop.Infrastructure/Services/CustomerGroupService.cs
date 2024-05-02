using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Schemas.Base;

namespace OnlineShop.Infrastructure.Services
{


  public class CustomerGroupService : ICustomerGroup
  {

    private readonly DataContext context;
    public CustomerGroupService(DataContext _ctx)
    {
      context = _ctx;
    }

    public List<CustomersGroups> Creates(List<CustomersGroups> customersGroups)
    {
      context.CustomerGroups.AddRange(customersGroups);
      context.SaveChanges();
      return customersGroups;
    }
  }
}
