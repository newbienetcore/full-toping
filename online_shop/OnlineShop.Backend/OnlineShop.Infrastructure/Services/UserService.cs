using OnlineShop.Core.Schemas.Base;
using OnlineShop.Core.Interfaces;

namespace OnlineShop.Infrastructure.Services
{

    public class UserService : IUserService
    {

        private readonly IDataContext context;

        public UserService(IDataContext _ctx)
        {
            this.context = _ctx;
        }

        public UserSchema UpdateLoginTime(int userId)
        {
            UserSchema user = context.Users.Find(userId);
            user.LastLogin = DateTime.UtcNow;
            return user;
        }

        public UserSchema SetRefreshToken(string refreshToken, int userId)
        {
            UserSchema user = context.Users.Find(userId);
            user.HashRefreshToken = refreshToken;
            return user;
        }

        public List<UserSchema> Get(string name)
        {
            List<UserSchema> users = context.Users.Where(u => u.UserName.Equals(name)).ToList();
            return users;
        }

        public bool CheckPermissionAction(int userId, string endPoint)
        {
            // Stopwatch sw = new Stopwatch(); 
            // sw.Start();

            PermSchema perm = (from p in context.Perms
                               join gp in context.GroupsPerms on p.Id equals gp.PermId
                               join g in context.Groups on gp.GroupId equals g.Id
                               join ug in context.UsersGroups on g.Id equals ug.GroupId
                               where ug.UserId == userId && p.Action == endPoint
                               select p).FirstOrDefault();
            // sw.Stop();

            return perm != null;
        }

        public void UpdateAfterLogin(int userId, string refreshToken)
        {
            UserSchema u = context.Users.Find(userId);
            u.HashRefreshToken = refreshToken;
            u.LastLogin = DateTime.UtcNow;
            context.SaveChanges();
        }
    }
}
