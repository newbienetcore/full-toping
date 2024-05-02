using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using SharedKernel.Core;

namespace SharedKernel.Providers;

public class CloudflareStorageProvider : ICloudflareStorageProvider, IDisposable
{
    // https://developers.cloudflare.com/r2/examples/aws/aws-sdk-net/
    private readonly AmazonS3Client _client;
    private const int expiryTime = 365; // days;

    public CloudflareStorageProvider()
    {
        _client = new AmazonS3Client(DefaultCloudflareConfig.AccessKey, DefaultCloudflareConfig.SecretKey, new AmazonS3Config
        {
            ServiceURL = DefaultCloudflareConfig.ServiceURL,
            RegionEndpoint = RegionEndpoint.APSoutheast1,
            RetryMode = RequestRetryMode.Standard,
            MaxErrorRetry = DefaultCloudflareConfig.MaxErrorRetry
        });
    }
    
    public async Task<UploadResponse> UploadAsync(UploadRequest model, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<UploadResponse>> UploadAsync(List<UploadRequest> models, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
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

    public async Task<List<DownloadResponse>> DownloadDirectoryAsync(string directory, string version = "", CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<DownloadResponse>> DownloadPagingAsync(string directory, int pageIndex, int pageSize, string version = "",
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    
    public void Dispose()
    {
        _client.Dispose();
    }
}