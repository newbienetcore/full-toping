using OnlineShop.Core;
using OnlineShop.Core.Schemas.Base;
using OnlineShop.Core.Interfaces;
using OnlineShop.Utils;

namespace UseCase.Permission.Crud
{
    public class CrudPermFlow
    {
        private readonly IDataContext dbContext;
        public CrudPermFlow(IDataContext ctx)
        {
            dbContext = ctx;
        }
        public Response List()
        {
            var groups = dbContext.Perms.ToList();
            return new Response(Message.SUCCESS, groups);
        }

        public async Task<Response> Create(PermSchema perm)
        {
            var result = dbContext.Perms.Add(perm);
            dbContext.SaveChanges();
            return new Response(Message.SUCCESS, result);
        }

        public async Task<Response> Update(PermSchema perm)
        {
            var result = dbContext.Perms.Update(perm);
            dbContext.SaveChanges();
            return new Response(Message.SUCCESS, result);
        }

        public Response Deletes(int[] ids)
        {
            var selectedItems = dbContext.Perms.Where(item => ids.Contains((int)item.GetType().GetProperty("Id").GetValue(item))).ToList();
            dbContext.Perms.RemoveRange(selectedItems);
            dbContext.SaveChanges();
            return new Response(Message.SUCCESS, ids);
        }
    }
}
