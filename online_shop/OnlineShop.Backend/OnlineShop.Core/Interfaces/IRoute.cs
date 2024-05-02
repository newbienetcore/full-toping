using OnlineShop.Core.Models;

namespace OnlineShop.Core.Interfaces
{
    public interface IRoute
    {
        IEnumerable<RouterModel> Get();
    }
}
