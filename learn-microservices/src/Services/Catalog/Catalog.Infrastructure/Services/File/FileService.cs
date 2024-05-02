using Catalog.Application.Services;
using Catalog.Infrastructure.Configs;
using Microsoft.AspNetCore.Http;
using SharedKernel.Runtime.Exceptions;

namespace Catalog.Infrastructure.Services;

public class FileService : IFileService
{
    public async Task CheckAcceptFileExtensionAndThrow(IFormFile file)
    {
        if (FirebaseConfig.AcceptExtensions.Contains("*"))
        {
            return;
        }
        
        if (!FirebaseConfig.AcceptExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
        {
            throw new BadRequestException($"File extension is not valid: {file.FileName}");
        }
    }

    public string GetFileUrl(string fileName)
    {
        return $"{FirebaseConfig.BaseUrl}/{FirebaseConfig.StorageBucket}/{fileName}";
    }
}