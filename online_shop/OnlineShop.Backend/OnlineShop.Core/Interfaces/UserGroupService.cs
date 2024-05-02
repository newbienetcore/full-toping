using OnlineShop.Core;
using OnlineShop.Core.Schemas.Base;
namespace OnlineShop.Core.Interfaces
{

    public interface IUserGroup
    {
        List<UsersGroups> Creates(List<UsersGroups> usersGroups);
    }
 
}
