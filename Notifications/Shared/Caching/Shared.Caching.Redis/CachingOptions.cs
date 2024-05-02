namespace Shared.Caching.Redis;

public class CachingOptions
{
    public string ConnectionString { get; set; }
    public string InstanceName { get; set; }
    public int DatabaseIndex { get; set; } = 0;
}