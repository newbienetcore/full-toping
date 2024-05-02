using OnlineShop.Core;
using OnlineShop.Core.Schemas.Base;

namespace OnlineShop.Core.Interfaces
{

  public interface ICustomerGroup
  {
    List<CustomersGroups> Creates(List<CustomersGroups> customersGroups);
  }
}
