using Microsoft.AspNetCore.Mvc;
using OnlineShop.Core;
using OnlineShop.Core.Interfaces;
using OnlineShop.UseCases.Category.Crud;
using OnlineShop.Utils;

namespace OnlineShop.Application.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        CrudCategoryFlow categoryFlow;
        public CategoryViewComponent(IDataContext ctx)
        {
            categoryFlow = new CrudCategoryFlow(ctx);
        }
        public IViewComponentResult Invoke()
        {
            Response data = categoryFlow.ListAndCount();
            if (data.Status == Message.SUCCESS)
            {
                return View(data.Result);
            }
            return View();
        }
    }
}
