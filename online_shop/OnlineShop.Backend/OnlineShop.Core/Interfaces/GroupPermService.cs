using OnlineShop.Core;
using OnlineShop.Core.Schemas.Base;
namespace OnlineShop.Core.Interfaces
{

    public interface IGroupPerm
    {
        List<GroupPerm> Creates(List<GroupPerm> perms);
    } 
}
