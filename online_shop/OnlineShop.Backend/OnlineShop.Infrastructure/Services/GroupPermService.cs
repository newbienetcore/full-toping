using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Schemas.Base;
namespace OnlineShop.Infrastructure.Services
{

    public class GroupPermService :  IGroupPerm
    {

        private readonly DataContext context;
        public GroupPermService(DataContext _ctx) 
        {
            context = _ctx;
        }

        public List<GroupPerm> Creates(List<GroupPerm> groupsPerms)
        {
            context.GroupsPerms.AddRange(groupsPerms);
            context.SaveChanges();
            return groupsPerms;
        }
    }
}
