

using OnlineShop.Core.Models;
using OnlineShop.Application.UseCases.User.Crud.Presenter;
using OnlineShop.Core.Interfaces;
using OnlineShop.WebApi.Controllers;
using static OnlineShop.Utils.CtrlUtil; 
using AutoMapper;
using Org.BouncyCastle.Crypto;
using OnlineShop.Core.Schemas.Base;

namespace OnlineShop.WebApi.Routers
{
    public class UserRouter : IRoute
    {
        private readonly UserCtrl userCtrl;
        public UserRouter(IDataContext ctx, IMapper mapper)
        {
            userCtrl = new UserCtrl(mapper, ctx);
        }

        public IEnumerable<RouterModel> Get()
        {
            List<RouterModel> routers = new List<RouterModel>();
            var userGet = new RouterModel()
            {
                Method = "GET",
                Module = "Users",
                Path = "users",
                ProfileType = RoleType.ADMIN_PROFILE,
                Action = async () => await userCtrl.GetAsync()
            };
            var userCreate = new RouterModel()
            {
                Method = "POST",
                Module = "Users",
                Path = "users",
                ProfileType = RoleType.ADMIN_PROFILE,
                Action = async (UserSchema model) => await userCtrl.CreateAsync(model)
            };
            var userUpdate = new RouterModel()
            {
                Method = "POST",
                Module = "Users",
                Path = "users/update",
                ProfileType = RoleType.ADMIN_PROFILE,
                Action = async (UpdateUserPresenter model) => await userCtrl.UpdateAsync(model)
            };
            var userdelete = new RouterModel()
            {
                Method = "POST",
                Module = "Users",
                Path = "users/ids",
                ProfileType = RoleType.ADMIN_PROFILE,
                Action = async (int[] ids) => await userCtrl.DeletesAsync(ids)
            };

            routers.Add(userCreate);
            routers.Add(userGet);
            routers.Add(userUpdate);
            routers.Add(userdelete);
            return routers;
        }
    }
}
