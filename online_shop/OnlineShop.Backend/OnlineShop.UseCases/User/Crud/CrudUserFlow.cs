using OnlineShop.Utils;
using OnlineShop.Core.Schemas.Base;
using OnlineShop.Core;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Business.Rule;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.UseCases.User.Crud
{

    public class CrudUserFlow
    {
        private readonly IDataContext dbContext;
        public CrudUserFlow(IDataContext ctx)
        {
            dbContext = ctx;
        }

        public async Task<Response> ListAsync()
        {
            var query = dbContext.Users;
            var users = await query.Where(u => u.Id != UserRule.ADMIN_ID).ToListAsync();
            return new Response(Message.SUCCESS, users);
        }

        public async Task<Response> CreateAsync(UserSchema user)
        {
            user.Password = JwtUtil.MD5Hash(UserRule.DEFAULT_PASSWORD);
            var result = await dbContext.Users.AddAsync(user);
            dbContext.SaveChanges();
            return new Response(Message.SUCCESS, result);
        }

        public async Task<Response> UpdateAsync(UserSchema user)
        {
            var result = dbContext.Users.Update(user);
            dbContext.SaveChanges();
            return new Response(Message.SUCCESS, result);
        }

        public async Task<Response> DeletesAsync(int[] ids)
        {
            var selectedItems = dbContext.Users.Where(item => ids.Contains((int)item.GetType().GetProperty("Id").GetValue(item))).ToList();
            dbContext.Users.RemoveRange(selectedItems);
            dbContext.SaveChanges();
            return new Response(Message.SUCCESS, ids);
        }

    }
}
