using OnlineShop.Core.Schemas.Base;
namespace OnlineShop.Core.Interfaces
{
    public interface IUserService
    {
        UserSchema SetRefreshToken(string refreshToken, int userId);
        UserSchema UpdateLoginTime(int userId);
        List<UserSchema> Get(string name);
        bool CheckPermissionAction(int user, string endPoint);
        void UpdateAfterLogin(int userId, string refreshToken);
    }


}
