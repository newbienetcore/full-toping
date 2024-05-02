using System.IdentityModel.Tokens.Jwt;
using OnlineShop.Core;
using OnlineShop.Core.Schemas.Base;
using OnlineShop.Core.Interfaces;
using OnlineShop.Utils;

namespace OnlineShop.UseCases.Auth
{
    public class AuthFlow
    {
        private readonly IDataContext dbContext;
        private readonly IUserService userService;
        public AuthFlow(IDataContext ctx, IUserService _userService)
        {
            dbContext = ctx;
            userService = _userService;
        }

        public Response Login(string username, string password, byte[] secretKey)
        {
            List<UserSchema> users = dbContext.Users.Where(u => u.UserName.Equals(username)).ToList();

            UserSchema user = users.FirstOrDefault();
            if (user == null)
            {
                return new Response(Message.ERROR, new { });
            }
            bool isMatched = JwtUtil.Compare(password, user.Password);
            if (!isMatched)
            {
                return new Response(Message.ERROR, new { });
            }
            string accessToken = JwtUtil.GenerateAccessToken(user.Id, secretKey);
            string refreshToken = JwtUtil.GenerateRefreshToken();
            userService.UpdateAfterLogin(user.Id, refreshToken);
            return new Response(Message.SUCCESS, new { AccessToken = accessToken, RefreshToken = refreshToken, User = user });
        }

        public Response RefreshToken(string accessToken, string refreshToken, byte[] secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(accessToken);
            var userCredentialString = jwtToken.Claims.First(x => x.Type == "id").Value;
            int userId = Int32.Parse(userCredentialString);
            UserSchema u = dbContext.Users.Find(userId);
            bool isMatched = u.HashRefreshToken.Equals(refreshToken);
            if (!isMatched)
            {
                return new Response(Message.ERROR, new { });
            }

            var newToken = JwtUtil.GenerateAccessToken(userId, secretKey);
            var newRefreshToken = JwtUtil.GenerateRefreshToken();
            return new Response(Message.SUCCESS, new
            {
                AccessToken = newToken,
                RefreshToken = newRefreshToken
            });

        }
    }
}
