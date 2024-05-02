using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Schemas.Base;
using OnlineShop.Core.Interfaces;

namespace OnlineShop.Infrastructure.Services
{

    public class PermService :  IPerm
    {

        private readonly IDataContext context;
        public PermService(IDataContext _ctx)
        {
            context = _ctx;
        }

        public List<PermSchema> GetPerms(int userId)
        {
            List<PermSchema> perms = (from p in context.Perms
                                join gp in context.GroupsPerms on p.Id equals gp.PermId
                                join g in context.Groups on gp.GroupId equals g.Id
                                join ug in context.UsersGroups on g.Id equals ug.GroupId
                                where ug.UserId == userId
                                select new PermSchema { Id = p.Id, Module = p.Module, Action = p.Action, Title = p.Title }).ToList();
            return perms;
        }

    }
}
