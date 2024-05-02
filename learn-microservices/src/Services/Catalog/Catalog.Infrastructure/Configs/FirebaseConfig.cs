using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Catalog.Infrastructure.Configs;

public class FirebaseConfig
{
    public static string ApiKey { get; private set; }
    public static string AuthDomain { get; private set; }
    public static string ProjectId { get; private set; }
    public static string StorageBucket { get; private set; }
    public static string MessagingSenderId { get; private set; }
    public static string AppId { get; private set; }
    public static string MeasurementId { get; private set; }
    
    [JsonIgnore]
    public static string Root { get; private set; }
    
    [JsonIgnore]
    public static string BaseUrl { get; set; }
    
    [JsonIgnore]
    public static List<string> AcceptExtensions { get; private set; }
    
    public static void SetConfig(IConfiguration configuration)
    {
        ApiKey = configuration.GetRequiredSection("FirebaseConfig:ApiKey").Value;
        AuthDomain = configuration.GetRequiredSection("FirebaseConfig:AuthDomain").Value;
        ProjectId = configuration.GetRequiredSection("FirebaseConfig:ProjectId").Value;
        StorageBucket = configuration.GetRequiredSection("FirebaseConfig:StorageBucket").Value;
        MessagingSenderId = configuration.GetRequiredSection("FirebaseConfig:MessagingSenderId").Value;
        AppId = configuration.GetRequiredSection("FirebaseConfig:AppId").Value;
        MeasurementId = configuration.GetRequiredSection("FirebaseConfig:MeasurementId").Value;
        Root = configuration.GetRequiredSection("FirebaseConfig:Root").Value;
        BaseUrl = configuration.GetRequiredSection("FirebaseConfig:BaseUrl").Value;
        AcceptExtensions = configuration.GetRequiredSection("FirebaseConfig:AcceptExtensions").Value.Split(",").ToList();
    }
    
}