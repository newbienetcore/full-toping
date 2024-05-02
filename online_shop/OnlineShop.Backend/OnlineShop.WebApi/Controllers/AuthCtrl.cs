using OnlineShop.Core;
using OnlineShop.Infrastructure.Services;
using OnlineShop.UseCases.Auth;
using OnlineShop.Core.Interfaces;

namespace OnlineShop.WebApi.Controllers
{
    public class AuthCtrl
    {
        private readonly AuthFlow workflow;
        private readonly byte[] secretKey;
        public AuthCtrl(IDataContext ctx, byte[] _secretKey)
        {
            secretKey = _secretKey;
            workflow = new AuthFlow(ctx, new UserService(ctx));
        }
        public IResult Login(string username, string password)
        {
            Response response = workflow.Login(username, password, secretKey);
            return Results.Ok(response);
        }

        public IResult RefreshToken(string accessToken, string refreshToken)
        {
            Response response = workflow.RefreshToken(accessToken, refreshToken, secretKey);
            return Results.Ok(response);
        }
    }
}
