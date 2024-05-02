using OnlineShop.Core.Models;
using OnlineShop.WebApi.Controllers;
using static OnlineShop.Utils.CtrlUtil;
using OnlineShop.Core.Interfaces;

namespace OnlineShop.WebApi.Routers
{
    public class PermRouter: IRoute
    {
        private readonly PermCtrl permCtrl;
        public PermRouter(IDataContext ctx)
        {
            permCtrl = new PermCtrl(ctx);
        }
        public IEnumerable<RouterModel> Get()
        {
            List<RouterModel> routers = new List<RouterModel>();
            var loginRouter = new RouterModel()
            {
                Method      = "GET",
                Module      = "Permissions",
                Path        = "permissions",
                ProfileType = RoleType.ADMIN_PROFILE,
                Action      = async () => await permCtrl.GetAsync()
            };
            routers.Add(loginRouter);
            return routers;
        }
    }
}
