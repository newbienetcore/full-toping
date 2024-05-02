using OnlineShop.Core.Models;
using OnlineShop.Core.Interfaces;
using OnlineShop.WebApi.Controllers;
using static OnlineShop.Utils.CtrlUtil;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Schemas;

namespace OnlineShop.WebApi.Routers
{
    public class CategoryRouter : IRoute
    {
        private readonly CategoryCtrl categoryCtrl;
        public CategoryRouter(IDataContext ctx)
        {
            categoryCtrl = new CategoryCtrl(ctx);
        }
        public IEnumerable<RouterModel> Get()
        {
            List<RouterModel> routers = new List<RouterModel>();
            var categoryRead = new RouterModel()
            {
                Method      = "GET",
                Module      = "Categories",
                Path        = "categories",
                ProfileType = RoleType.ADMIN_PROFILE,
                Action      = async () => categoryCtrl.Get()
            };
            
            routers.Add(categoryRead);
            return routers;
        } 
    }
}
