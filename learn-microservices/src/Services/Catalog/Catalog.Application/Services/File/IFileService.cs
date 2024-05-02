using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Services;

public interface IFileService
{
    Task CheckAcceptFileExtensionAndThrow(IFormFile file);
    
    string GetFileUrl(string fileName);
}