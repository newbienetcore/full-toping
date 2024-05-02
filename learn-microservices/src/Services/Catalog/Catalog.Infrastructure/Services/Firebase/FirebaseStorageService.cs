using Catalog.Application.Services;
using Catalog.Infrastructure.Configs;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharedKernel.Log;
using SharedKernel.Providers;
using Object = Google.Apis.Storage.v1.Data.Object;

namespace Catalog.Infrastructure.Services;

public class FirebaseStorageService : IFirebaseStorageService
{
    private readonly StorageClient _storageClient;
    public FirebaseStorageService(IConfiguration configuration)
    {
        GoogleCredential credential = GoogleCredential.FromFile("e-commerce-3f9f0-firebase-adminsdk-my54u-4515f2bcf9.json");
        _storageClient = StorageClient.Create(credential);
    }
    
    public async Task<UploadResponse> UploadAsync(UploadRequest model, CancellationToken cancellationToken = default)
    {
        try
        {
            var currentFileName = $"{Guid.NewGuid()}{model.FileExtension}";
            var filePath = string.IsNullOrEmpty(model.Prefix) 
                ? $"{FirebaseConfig.Root}/{currentFileName}" 
                : $"{FirebaseConfig.Root}/{model.Prefix}/{currentFileName}";
            
            var request = new Object()
            {
                Name = filePath,
                Bucket = FirebaseConfig.StorageBucket,
                Size = (ulong?)model.Size,
            };
            
            var uploadObjectOptions = new UploadObjectOptions
            {
                PredefinedAcl = PredefinedObjectAcl.PublicRead,
                ChunkSize = null
            };
            
            await _storageClient.UploadObjectAsync(
                destination: request,
                source: model.Stream,
                options: uploadObjectOptions,
                cancellationToken : cancellationToken
                );

            return new UploadResponse()
            {
                OriginalFileName = model.FileName,
                CurrentFileName = filePath,
                FileExtension = model.FileExtension,
                Size = model.Size,
                Prefix = model.Prefix,
            };
        }
        catch (Exception ex)
        {
            Logging.Error(ex, "firebase upload failed");
            return new UploadResponse()
            {
                Success = false,
                OriginalFileName = model.FileName,
                ErrorMessage = ex.Message,
            };
        }
    }

    public async Task<List<UploadResponse>> UploadAsync(List<UploadRequest> models, CancellationToken cancellationToken = default)
    {
        var responses = await Task.WhenAll(models.Select(model => this.UploadAsync(model, cancellationToken)));
        return responses.ToList();
    }

    public async Task<DownloadResponse> DownloadAsync(string fileName, string version = "", CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<DownloadResponse>> DownloadAsync(List<string> fileNames, string version = "", CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(string fileName, string version = "", CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}