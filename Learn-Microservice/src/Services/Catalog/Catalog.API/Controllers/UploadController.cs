using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

public class UploadController : ApiControllerBase
{
    private readonly IWebHostEnvironment _env;
    public UploadController(IWebHostEnvironment env)
    {
        _env = env ?? throw new ArgumentNullException(nameof(env));
    }

    [HttpPost]
    [Route("api/files/upload")]
    public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file is null || file.Length < 0)
            return BadRequest();
        
        string fileName = Path.GetFileNameWithoutExtension(file.FileName);
        string fileExtension = Path.GetExtension(file.FileName);
        string newFileName = fileName + "_" + Guid.NewGuid() + fileExtension;
        string filePath  = Path.Combine(_env.WebRootPath, "uploads", newFileName);
        using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream, cancellationToken);
        return Ok(filePath.Trim());
    }
}