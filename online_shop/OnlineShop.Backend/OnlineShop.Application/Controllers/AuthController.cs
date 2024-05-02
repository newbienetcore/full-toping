using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Helpers;
using OnlineShop.Application.UseCases.Auth;
using OnlineShop.Application.UseCases.Customer;
using OnlineShop.Application.ViewModels;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Schemas;
using OnlineShop.Infrastructure;
using OnlineShop.Infrastructure.Services;
using OnlineShop.UseCases.Auth;
using OnlineShop.UseCases.Customer.Crud;
using OnlineShop.Utils;
using System.Security.Claims;
using System.Text;

namespace OnlineShop.Application.Controllers
{
	public class AuthController : Controller
	{
		private readonly string SessionKeyUsername = "_Username";
		CrudCustomerFlow authFlow;
		public AuthController(IDataContext ctx)
		{
			authFlow = new CrudCustomerFlow(ctx, new CustomerService(ctx));
		}

		public IActionResult Login()
		{
			ClaimsPrincipal claimuser = HttpContext.User;
			if (claimuser.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}
			return View();
		}
		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginVM customer)
		{
			var response = authFlow.Login(customer.UserName, customer.Password);

			if (!ModelState.IsValid || response.Status == Message.ERROR)
			{
				ViewData["ValidateMessage"] = "User name or password is not valid";
				return View("Login", customer);
			}
			List<Claim> claims = new List<Claim>()
				{
					new Claim(ClaimTypes.Name, customer.UserName)
				};
			ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var claimsprincicle = new ClaimsPrincipal(identity);
			await HttpContext.SignInAsync(claimsprincicle);


			var username = HttpContext.Session.Get<string>(SessionKeyUsername);

			if (string.IsNullOrEmpty(username))
			{
				HttpContext.Session.Set(SessionKeyUsername, customer.UserName);
			}
            return RedirectToAction("Index", "Home");
		}

		[HttpGet("Logout")]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return Redirect("/");
		}


	}
}
