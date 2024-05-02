namespace Caching;

public class RedisCacheSetting
{
    public const string SectionName = "Redis";
    public string ConnectionString { get; set; }
    public string InstanceName { get; set; }
    public int DatabaseIndex { get; set; } = 0;
}