using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application;

namespace Catalog.Api.Controllers.VersionOne;

[ApiVersion("1.0")]
public class OwnerController : BaseController
{
    [AllowAnonymous]
    [HttpGet("owner-information")]
    public async Task<IActionResult> GetAsync()
    {
        var result = new ApiSimpleResult()
        {
            Data = new
            {
                FullName = "Đỗ Chí Hùng",
                DateOfBirth = new DateTime(2002, 09, 06),
                Phone = "0976580418",
                Email = "dohung.csharp@gmail.com",
                Facebook = "https://www.facebook.com/dohungiy",
                MostBeautifulDay = "Ngày em đẹp nhất là ngày anh chưa có gì trong tay!"
            }
        };
        
        return Ok(result);
    }
}