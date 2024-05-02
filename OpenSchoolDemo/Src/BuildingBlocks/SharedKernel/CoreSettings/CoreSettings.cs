using Microsoft.Extensions.Configuration;
using Serilog;

namespace SharedKernel.Core;

public static class CoreSettings
{
    public static readonly bool IsSingleDevice = false;
    public static Dictionary<string, string> ConnectionStrings { get; private set; }

    public static DefaultLoggingConfig DefaultLoggingConfig { get; private set; }

    public static DefaultEmailConfig DefaultEmailConfig { get; private set; }
    
    public static DefaultCloudflareConfig DefaultCloudflareConfig { get; private set; }
    
    public static DefaultJWTConfig DefaultJWTConfig { get; private set; }
    
    public static List<string> Black3pKeywords { get; private set; }

    public static void SetConfig(IConfiguration configuration, ILogger logger)
    {
        SetConnectionStrings(configuration);
        SetJWTConfig(configuration);
        SetLoggingConfig(configuration, logger);
        SetEmailConfig(configuration);
        SetCloudflareConfig(configuration);
        SetBlack3pKeywords(configuration);
    }

    public static void SetConnectionStrings(IConfiguration configuration)
    {
        ConnectionStrings = configuration.GetRequiredSection("ConnectionStrings").Get<Dictionary<string, string>>();
    }
    
    public static void SetJWTConfig(IConfiguration configuration)
    {
        DefaultJWTConfig.SetDefaultJWTConfig(configuration);
    }
        
    public static void SetEmailConfig(IConfiguration configuration)
    {
        DefaultEmailConfig.SetConfig(configuration);
    }

    public static void SetLoggingConfig(IConfiguration configuration, ILogger logger)
    {
        DefaultLoggingConfig.SetConfig(configuration, logger);
    }
    

    public static void SetCloudflareConfig(IConfiguration configuration)
    {
        DefaultCloudflareConfig.SetConfig(configuration);
    }

    public static void SetBlack3pKeywords(IConfiguration configuration)
    {
        Black3pKeywords = configuration.GetValue<string>("Black3pKeyword").Split(",").ToList();
    }
}