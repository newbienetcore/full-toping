using AutoMapper;
using OnlineShop.Application.UseCases.Customer;
using OnlineShop.Application.UseCases.Customer.Crud.Presenter;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Models;
using OnlineShop.UseCases.Customer.Crud.Presenter;
using OnlineShop.WebApi.Controllers;
using static OnlineShop.Utils.CtrlUtil;

namespace OnlineShop.WebApi.Routers
{
    public class CustomerRouter
    {
        private readonly CustomerCtrl customerCtrl;
        public CustomerRouter(IDataContext ctx)
        {
            customerCtrl = new CustomerCtrl(ctx);
        }
        public IEnumerable<RouterModel> Get()
        {
            List<RouterModel> routers = new List<RouterModel>();
            var customerRead = new RouterModel()
            {
                Method = "GET",
                Module = "Customers",
                Path = "customers",
                ProfileType = RoleType.ADMIN_PROFILE,
                Action = async (string sortName = "Id", string sortType = "ASC", int cursor = 0, int pageSize = 10) => customerCtrl.List(sortName,sortType,cursor,pageSize)
            };
            var customerLogin = new RouterModel()
            {
                Method = "POST",
                Module = "Customer",
                Path = "customer/login",
                ProfileType = RoleType.PUBLIC_PROFILE,
                Action = async (HttpContext context,GetCustomerPresenter customer) => customerCtrl.Login(context, customer)
            };
            var customerRegister = new RouterModel()
            {
                Method = "POST",
                Module = "Customer",
                Path = "customer/register",
                ProfileType = RoleType.PUBLIC_PROFILE,
                Action = async (IMapper _mapper, CreateCustomerPresenter model) => customerCtrl.Register(_mapper, model)
            };
            var customerGetById = new RouterModel()
            {
                Method = "GET",
                Module = "Customer",
                Path = "customer/id",
                ProfileType = RoleType.ADMIN_PROFILE,
                Action = async (int id) => customerCtrl.Get(id)
            };
            var customerUpdate = new RouterModel()
            {
                Method = "PUT",
                Module = "Customer",
                Path = "customer",
                ProfileType = RoleType.ADMIN_PROFILE,
                Action = async (IMapper _mapper, int Id, UpdateCustomerPresenter model) => customerCtrl.Update(_mapper,Id,model)
            };
            var customerDelete = new RouterModel()
            {
                Method = "DELETE",
                Module = "Customer",
                Path = "customer",
                ProfileType = RoleType.ADMIN_PROFILE,
                Action = async (int id) => customerCtrl.Delete(id)
            };
            routers.Add(customerRead);
            routers.Add(customerLogin);
            routers.Add(customerRegister);
            routers.Add(customerGetById);
            routers.Add(customerUpdate);
            routers.Add(customerDelete);
            return routers;
        }

    }
}
