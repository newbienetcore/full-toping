using Microsoft.AspNetCore.Mvc; 
using OnlineShop.Core.Schemas; 
using OnlineShop.Core.Interfaces;
using OnlineShop.UseCases.Customer.Crud;
using OnlineShop.Infrastructure.Services;
using OnlineShop.Application.ViewModels;
using AutoMapper;
namespace OnlineShop.Application.Controllers
{
    public class CustomerController : Controller
    {
		private readonly IMapper _mapper;
        CrudCustomerFlow crudCustomerFlow;
        public CustomerController(IDataContext ctx, IMapper mapper)
		{
			crudCustomerFlow = new CrudCustomerFlow(ctx, new CustomerService(ctx));
			_mapper = mapper;
		}

		public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM customer)
        {
            if (customer.Phonenumber == 0)
            {
                ModelState.AddModelError("Phonenumber", "The Phonenumber field is required.");
            }

            if (!ModelState.IsValid)
            {
                return View("Register", customer);
            }
            var result = _mapper.Map<CustomerSchema>(customer);
            crudCustomerFlow.Create(result);
            return RedirectToAction("Index", "Home");
        }
    }
}
