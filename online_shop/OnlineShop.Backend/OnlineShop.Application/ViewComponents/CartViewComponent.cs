using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineShop.Application.Helpers;
using OnlineShop.Application.ViewModels;
using OnlineShop.Core;
using OnlineShop.Utils;
using System.Collections.Generic;
using System.Net.WebSockets;

namespace OnlineShop.Application.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public const string CartSessionKey = "cart";
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItemViewModel>>(CartSessionKey) ??new List< CartItemViewModel>();
            var producttotal = cart.Sum(p => p.Quantity);
            return View(producttotal);
        }
    }
}
