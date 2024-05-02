using OnlineShop.Core.Models;
using OnlineShop.Core.Interfaces;
using OnlineShop.WebApi.Controllers;
using static OnlineShop.Utils.CtrlUtil; 

namespace OnlineShop.WebApi.Routers
{
    public class GroupRouter: IRoute
    {
        private readonly GroupCtrl groupCtrl;
        public GroupRouter(IDataContext ctx)
        {
            groupCtrl = new GroupCtrl(ctx);
        }
        public IEnumerable<RouterModel> Get()
        {
            List<RouterModel> routers = new List<RouterModel>();
            var loginRouter = new RouterModel()
            {
                Method      = "GET",
                Module      = "Groups",
                Path        = "groups",
                ProfileType = RoleType.ADMIN_PROFILE,
                Action      = async () =>await groupCtrl.GetAsync()
            };
            routers.Add(loginRouter);
            return routers;
        } 
    }
}
