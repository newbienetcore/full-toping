using Microsoft.Extensions.Configuration;

namespace SharedKernel.Core;

public class DefaultCloudflareConfig
{
    public static string AccessKey { get; private set; }
    public static string SecretKey { get; private set; }
    public static string ServiceURL { get; private set; }
    public static int MaxErrorRetry { get; private set; }
    public static string Bucket { get; private set; }
    public static string Root { get; private set; }
    public static List<string> AcceptExtensions { get; private set; }

    public static void SetConfig(IConfiguration configuration)
    {
        AccessKey = configuration.GetRequiredSection("Cloudflare:AccessKey").Value;
        SecretKey = configuration.GetRequiredSection("Cloudflare:SecretKey").Value;
        ServiceURL = configuration.GetRequiredSection("Cloudflare:SecretKey").Value;
        MaxErrorRetry = int.Parse(configuration.GetRequiredSection("Cloudflare:SecretKey").Value);
        Bucket = configuration.GetRequiredSection("Cloudflare:Bucket").Value;
        Root = configuration.GetRequiredSection("Cloudflare:Root").Value;
        AcceptExtensions = configuration.GetRequiredSection("Cloudflare:AcceptExtensions").Value.Split(",").ToList();
    }
}