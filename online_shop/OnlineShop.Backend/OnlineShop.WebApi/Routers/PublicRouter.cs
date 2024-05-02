

using OnlineShop.Core.Models;
using OnlineShop.WebApi.Controllers;
using OnlineShop.Core.Interfaces;
using AutoMapper;
using OnlineShop.Utils;
using OnlineShop.Core.Interfaces;

namespace OnlineShop.WebApi.Routers
{
    public class PublicRouter: IRoute
    {
        private readonly SeedCtrl seedCtrl;
        private readonly AuthCtrl authCtrl;
        private readonly byte[] secretKey;
        private readonly IMapper mapper;
        public PublicRouter(IDataContext ctx, IMapper _mapper, byte[] _secretKey)
        {
            secretKey = _secretKey;
            mapper = _mapper;
            seedCtrl = new SeedCtrl(ctx, secretKey, mapper);
            authCtrl = new AuthCtrl(ctx, secretKey);
        }
        public IEnumerable<RouterModel> Get()
        {
            List<RouterModel> routers = new List<RouterModel>();
            var seedRouter = new RouterModel()
            {
                Method = "GET",
                Module = "Seed",
                Path = "seed",
                ProfileType = PermissionUtil.PUBLIC_PROFILE,
                Action = async () => await seedCtrl.SyncAllPerm()
            };

            var loginRouter = new RouterModel()
            {
                Method = "POST",
                Module = "Auth",
                Path = "auth/login",
                ProfileType = PermissionUtil.PUBLIC_PROFILE,
                Action = (string username, string password) => authCtrl.Login(username, password)
            };

            var refreshTokenRouter = new RouterModel()
            {
                Method = "GET",
                Module = "Auth",
                Path = "auth/refresh-token",
                ProfileType = PermissionUtil.PUBLIC_PROFILE,
                Action = (string accessToken, string refreshToken) => authCtrl.RefreshToken(accessToken, refreshToken)
            };

            routers.Add(seedRouter);
            routers.Add(loginRouter);
            routers.Add(refreshTokenRouter);
            return routers;
        }
    }
}
