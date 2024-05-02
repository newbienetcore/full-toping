using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Application.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index(int id)
        {
            return View();
        }
    }
}
