using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineShop.Application.Helpers;
using OnlineShop.Application.ViewModels;
using OnlineShop.Core.Interfaces;
using OnlineShop.Utils;
using OnlineShopS.UseCases.Product.Crud;

namespace OnlineShop.Application.Controllers
{
    [Authorize]
    public class ShopCartController : Controller
    {
        private readonly IMapper _mapper;
        public const string CartSessionKey = "cart";
        CrudProductFlow crudProductFlow;
        public ShopCartController(IDataContext ctx, IMapper mapper)
        {
            crudProductFlow = new CrudProductFlow(ctx);
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View(GetCartItems());
        }
        
        public IActionResult AddToCart(int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError(nameof(id), "ID sản phẩm không hợp lệ");
                return BadRequest(ModelState);
            }

            var data = crudProductFlow.Get(id);
            var product = _mapper.Map<ProductVM>(data.Result);

            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.Product.Id == id);
            if (cartitem != null)
            {
                cartitem.Quantity++;
            }
            else
            {
                cart.Add(new CartItemViewModel() { Quantity = 1, Product = product });
            }
            SaveCartSession(cart);
            return RedirectToAction("Index", "Shop");
        }
        List<CartItemViewModel> GetCartItems()
        {

            var cart = HttpContext.Session.Get<List<CartItemViewModel>>(CartSessionKey) ?? new List<CartItemViewModel>();
            return cart;
        }

        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CartSessionKey);
        }
        void SaveCartSession(List<CartItemViewModel> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CartSessionKey, jsoncart);
        }

        public IActionResult RemoveCart(int productId)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.Product.Id == productId);
            if (cartitem != null)
            {
                cart.Remove(cartitem);
            }

            SaveCartSession(cart);
            return RedirectToAction("Index");
        }
        public IActionResult UpdateCart(Dictionary<int, int> quantities)
        {
            foreach (var productId in quantities.Keys)
            {
                var quantity = quantities[productId];
                var cart = GetCartItems();
                var cartitem = cart.Find(p => p.Product.Id == productId);
                if (cartitem != null)
                {
                    cartitem.Quantity = quantity;
                }
                SaveCartSession(cart);
            }
            // return Ok();
            return RedirectToAction("Index");
        }

    }
}
