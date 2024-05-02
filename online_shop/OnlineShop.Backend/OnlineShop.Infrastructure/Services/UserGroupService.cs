using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Schemas.Base;
namespace OnlineShop.Infrastructure.Services
{
    public class UserGroupService :   IUserGroup
    {

        private readonly DataContext context;
        public UserGroupService(DataContext _ctx)
        {
            context = _ctx;
        }

        public List<UsersGroups> Creates(List<UsersGroups> usersGroups)
        {
            context.UsersGroups.AddRange(usersGroups);
            context.SaveChanges();
            return usersGroups;
        }
    }
}
