using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.ViewModels;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Models;
using OnlineShop.Infrastructure.Services;
using OnlineShop.UseCases.Product.GetProductList;
using OnlineShop.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OnlineShop.Application.Controllers
{
    public class HomeController : Controller
    {
        GetProductListFlow _getProductListFlow;
        private readonly IMapper _mapper;
        public HomeController(IDataContext ctx, IMapper mapper)
        {
            _getProductListFlow = new GetProductListFlow(ctx, new ProductService(ctx));
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            ViewData["new"] = GetProducts(null, 8); ;
            ViewData["trend"] = GetProducts(ProductStatus.Trend, 3);
            ViewData["bestseller"] = GetProducts(ProductStatus.BestSeller, 3);
            ViewData["feature"] = GetProducts(ProductStatus.Featured, 3);

            return View();
        }
        public List<ProductVM> GetProducts(ProductStatus? status, int? count)
        {
            var products = _getProductListFlow.GetProducts(status, count);

            if (products.Status == Message.SUCCESS)
            {
                var resultProducts = _mapper.Map<List<ProductVM>>(products.Result);
                foreach (var product in resultProducts)
                {
                    if (product.Discount >= 0 && product.Discount <= 1)
                    {
                        product.PriceAfterDiscount = (float)Math.Round(product.Price * (1 - product.Discount), 2);
                    }
                    else
                    {
                        product.PriceAfterDiscount = product.Price;
                    }
                }

                return resultProducts;
            }
            return new List<ProductVM>();
        }

    }
}
