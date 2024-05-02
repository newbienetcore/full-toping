using Microsoft.AspNetCore.Mvc;
using OnlineShop.Core.Interfaces;
using OnlineShop.Infrastructure.Services;
using OnlineShop.UseCases.Customer.Crud;

namespace OnlineShop.Application.Controllers
{
    public class ContactController : Controller
    {
        CrudCustomerFlow crudCustomerFlow;
        public ContactController(IDataContext ctx)
        {
            crudCustomerFlow = new CrudCustomerFlow(ctx, new CustomerService(ctx));
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
