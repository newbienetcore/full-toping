using System;
using System.Collections.Generic;
using System.Net.Http;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.ViewModels;
using OnlineShop.Core.Interfaces;
using OnlineShop.Infrastructure.Services;
using OnlineShop.UseCases.Category.Crud;
using OnlineShop.UseCases.Product.GetProductList;
using OnlineShop.Utils;
using OnlineShopS.UseCases.Product.Crud;

namespace OnlineShop.Application.Controllers
{
	public class ShopController : Controller
	{
		private readonly IMapper _mapper;
		CrudCategoryFlow categoryFlow;
		GetProductListFlow getProductListFlow;
		CrudProductFlow productListFlow;
		public ShopController(IDataContext ctx, IMapper mapper)
		{
			categoryFlow = new CrudCategoryFlow(ctx);
			getProductListFlow = new GetProductListFlow(ctx,new ProductService(ctx));
			productListFlow = new CrudProductFlow(ctx);
			_mapper = mapper;
		}
		public IActionResult Index()
		{
			var data = getProductListFlow.List();
			if (data.Status == Message.SUCCESS)
			{
				var result = _mapper.Map<List<ProductVM>>(data.Result);
				return View(result);
			}
			return View();
		}
		public IActionResult ListByCategory(int id)
		{
			var data = categoryFlow.ListProductById(id);
			if (data.Status == Message.SUCCESS)
			{

				var result = _mapper.Map<List<ProductVM>>(data.Result);
				return View("Index", result);
			}
			return View("Index");
		}

		public IActionResult Search(string search)
		{
			var data = getProductListFlow.List(search);
			if (data.Status == Message.SUCCESS)
			{
				var result = _mapper.Map<List<ProductVM>>(data.Result);
				View("Index", result);
			}
			return View("Index");
		}
		public IActionResult Detail(int id)
		{
			var data = productListFlow.Get(id);
			if (data.Status == Message.SUCCESS)
			{
				var result = _mapper.Map<ProductVM>(data.Result);
                if (result.Discount >= 0 && result.Discount <= 1)
                {
                    result.PriceAfterDiscount = (float)Math.Round(result.Price * (1 - result.Discount), 2);
                }
                else
                {
                    result.PriceAfterDiscount = result.Price;
                }
                View(result);
			}
			return View();
		}
	}
}
