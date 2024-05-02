using AutoMapper;
using OnlineShop.Application.UseCases.Auth;
using OnlineShop.Application.UseCases.Customer;
using OnlineShop.Application.UseCases.Customer.Crud.Presenter;
using OnlineShop.Core;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Schemas;
using OnlineShop.Infrastructure.Services;
using OnlineShop.UseCases.Customer.Crud;
using OnlineShop.UseCases.Customer.Crud.Presenter;
using OnlineShop.Utils;

namespace OnlineShop.WebApi.Controllers
{
    public class CustomerCtrl
    {
        private readonly CrudCustomerFlow workflow;
        public CustomerCtrl(IDataContext ctx)
        {
            workflow = new CrudCustomerFlow(ctx,new CustomerService(ctx));
        }
        public IResult Register(IMapper _mapper, CreateCustomerPresenter model)
        {
            CustomerSchema customer = _mapper.Map<CustomerSchema>(model);
            Response response = workflow.Create(customer);
            if(response.Status == Message.ERROR)
            {
                return Results.BadRequest();
            }
            return Results.Ok(response);
        }
        public IResult Login(HttpContext context, GetCustomerPresenter model)
        {
            Response response = workflow.Login(model.UserName, model.Password);
            if (response.Status == Message.ERROR)
            {
                return Results.Unauthorized();
            }
            LoginPresenter token = (LoginPresenter)response.Result;
            CookieOptions cookieOptions = JwtUtil.GetConfigOption();
            context.Response.Cookies.Append(PermissionUtil.ACCESS_TOKEN, token.AccessToken, cookieOptions);
            context.Response.Cookies.Append(PermissionUtil.REFRESH_TOKEN, token.RefreshToken, cookieOptions);
            return Results.Ok(response);
        }
        public IResult List(string sortName, string sortType, int cursor, int pageSize)
        {
            Response response = workflow.List();
            List<CustomerSchema> items = (List<CustomerSchema>)response.Result;
            if (sortName == "Id")
            {
                if (sortType == "ASC")
                {
                    items = items.OrderBy(item => item.Id).ToList();
                }
                else if (sortType == "DESC")
                {
                    items = items.OrderByDescending(item => item.Id).ToList();
                }
            }
            ResponsePresenter res = CtrlUtil.ApplyPaging<CustomerSchema, string>(cursor, pageSize, items);
            if (response.Status == Message.ERROR)
            {
                return Results.BadRequest();
            }
            res.Items = CrudCustomerPresenter.PresentList((List<CustomerSchema>)res.Items);
            return Results.Ok(res);
        }
        public IResult Get(int id)
        {
            Response response = workflow.Get(id);
            if (response.Status == Message.ERROR)
            {
                return Results.BadRequest();
            }

            return Results.Ok(response);
        }
        public IResult Update(IMapper _mapper, int Id, UpdateCustomerPresenter model)
        {
            CustomerSchema customer = _mapper.Map<CustomerSchema>(model);
            Response response = workflow.UpdateCustomer(Id, customer);

            if (response.Status == Message.ERROR)
            {
                return Results.BadRequest();
            }

            return Results.Ok(response);
        }
        public IResult Delete(int id)
        {
            Response response = workflow.Delete(id);
            if(response.Status == Message.ERROR)
            {
                return Results.BadRequest();
            }
            return Results.Ok(response);
        }
    }
}
