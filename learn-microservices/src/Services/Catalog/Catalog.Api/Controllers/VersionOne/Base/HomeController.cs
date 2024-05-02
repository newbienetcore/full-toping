using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers.VersionOne;

[ApiVersion("1.0")]
public class HomeController : BaseController
{
    [HttpGet]
    public IActionResult IndexAsync()
    {
        return Redirect("~/swagger");
    }
    
}