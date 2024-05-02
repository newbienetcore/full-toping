using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="section"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetOptions<T>(this IConfiguration configuration, string section) where T : class, new()
    {
        var options = new T();
        configuration.GetSection(section).Bind(options);

        return options;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>

    public static T GetOptions<T>(this IConfiguration configuration) where T : class, new()
        => GetOptions<T>(configuration, typeof(T).Name);
}