using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Helpers;
using OnlineShop.Application.ViewModels;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Models;
using OnlineShop.Core.Schemas;
using OnlineShop.Infrastructure.Services;
using OnlineShop.UseCases.Customer.Crud;
using OnlineShop.UseCases.Mail.SendMail;
using OnlineShop.UseCases.Order.Crud;
using OnlineShop.Utils;

namespace OnlineShop.Application.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        public const string CartSessionKey = "cart";
        private readonly string SessionKeyUsername = "_Username";
        private readonly IMapper _mapper;
        CrudCustomerFlow _customerFlow;
        CrudOrderFlow _orderflow;
        SendMailFlow _sendMailFlow;

        public CheckoutController(IDataContext ctx, IMapper mapper)
        {
            _mapper = mapper;
            _customerFlow = new CrudCustomerFlow(ctx, new CustomerService(ctx));
            _orderflow = new CrudOrderFlow(ctx);
            _sendMailFlow = new SendMailFlow(new MailService());
        }
        public IActionResult Index()
        {
            var model = new CheckoutViewModel
            {
                Cart = GetCartItems(),
                CustomerPlaceOrder = new CustomerPlaceOrder()
            };
            return View(model);
        }
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customername = HttpContext.User.Identity.Name;
                if (string.IsNullOrEmpty(customername))
                {
                    return RedirectToAction("Login", "Auth");
                }
                var getCustomerIdResponse = _customerFlow.GetUserIdByUserName(customername);
                if (getCustomerIdResponse.Status == Message.SUCCESS)
                {
                    if (getCustomerIdResponse.Result is int CustomerId)
                    {
                        model.CustomerPlaceOrder.CustomerID = CustomerId;
                        var order = _mapper.Map<OrderSchema>(model.CustomerPlaceOrder);
                        var cart = GetCartItems();//by session
                        var productorder = new List<ProductOrderSchema>();
                        foreach (var item in cart)
                        {
                            productorder.Add(new ProductOrderSchema
                            {
                                OrderID = order.Id,
                                ProductID = item.Product.Id,
                                Quantity = item.Quantity,
                                Price = item.Product.Price,
                                CreatedAt = DateTime.Now,
                            });
                        }
                        var res = await _orderflow.CreateOrderAndProductOrders(order, productorder);
                        if (res.Status == Message.SUCCESS)
                        {
                            var mailcontent = new MailContentModel()
                            {
                                To = model.CustomerPlaceOrder.Email,
                                Subject = "[OnlineShop] Xác nhận thanh toán thành công",
                                Body = GenerateContentEmail(),
                            };
                            HttpContext.Session.Remove(CartSessionKey);
                            //await _sendMailFlow.SendMail(mailcontent); 
                            return View();
                        }
                    }
                }
               
            }

            model.Cart = GetCartItems();
            return View("Index", model);
        }


        public string GenerateContentEmail()
        {
            var cart = GetCartItems();
            var total = cart.Sum(p => p.Product.Price * p.Quantity);
            var contentEmail = $@"
<!DOCTYPE html>
<html>
   <body>
      <div class='checkout__order'>
         <h5 style='
            border-bottom: 1px solid #d7d7d7;
            margin-bottom: 18px;
            color: #111111;
            font-weight: 600;
            text-transform: uppercase;
            border-bottom: 1px solid #e1e1e1;
            padding-bottom: 20px;
            margin-bottom: 25px;
            '>Your order</h5>
         <div class='checkout__order__product' style='
            border-bottom: 1px solid #d7d7d7;
            padding-bottom: 22px;
            '>
            <ul>
               <li style='
                  list-style: none;
                  font-size: 14px;
                  color: #444444;
                  font-weight: 500;
                  overflow: hidden;
                  margin-bottom: 14px;
                  line-height: 24px;
                  '>
                  <span class='top__text' style='
                     font-size: 16px;
                     color: #111111;
                     font-weight: 600;
                     float: left;
                     '>Product</span>
                  <span class='top__text__right' style='
                     font-size: 16px;
                     color: #111111;
                     font-weight: 600;
                     float: right;
                     '>Total</span>
               </li>";
            for (var index = 0; index < cart.Count; index++)
            {
                var item = cart[index];
                contentEmail += $@"<li style='
                  list-style: none;
                  font-size: 14px;
                  color: #444444;
                  font-weight: 500;
                  overflow: hidden;
                  margin-bottom: 14px;
                  line-height: 24px;
                  '>{item.Product.Name} <span style='
                  font-size: 14px;
                  color: #111111;
                  font-weight: 600;
                  float: right;
                  '>$ {item.Product.Price}</span></li>";
            }
            contentEmail += $@"
            </ul>
         </div>
         <div class='checkout__order__total' style='
            padding-top: 12px;
            border-bottom: 1px solid #d7d7d7;
            padding-bottom: 10px;
            margin-bottom: 25px;
            '>
            <ul>
               <li style='
                  list-style: none;
                  font-size: 14px;
                  color: #444444;
                  font-weight: 500;
                  overflow: hidden;
                  margin-bottom: 14px;
                  line-height: 24px;
                  '>Subtotal <span style='
                  font-size: 14px;
                  color: #111111;
                  font-weight: 600;
                  float: right;
                  '>$ {total}</span></li>
               <li style='
                  list-style: none;
                  font-size: 14px;
                  color: #444444;
                  font-weight: 500;
                  overflow: hidden;
                  margin-bottom: 14px;
                  line-height: 24px;
                  '>Total <span style='
                  font-size: 14px;
                  color: #111111;
                  font-weight: 600;
                  float: right;
                  '>$ {total}</span></li>
            </ul>
         </div>
      </div>
   </body>
</html>";
            return contentEmail;
        }

        List<CartItemViewModel> GetCartItems()
        {
            var cart = HttpContext.Session.Get<List<CartItemViewModel>>(CartSessionKey) ?? new List<CartItemViewModel>();
            return cart;
        }
    }
}
