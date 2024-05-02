using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace MessageBroker.Abstractions;

public class EventBusSubscriptionInfo
{
    public Dictionary<string, Type> EventTypes { get; } = [];

    public JsonSerializerOptions JsonSerializerOptions { get; } = new (DefaultJsonSerializerOptions);

    internal static readonly JsonSerializerOptions DefaultJsonSerializerOptions = new()
    {
        TypeInfoResolver = JsonSerializer.IsReflectionEnabledByDefault ? CreateDefaultTypeResolver() : JsonTypeInfoResolver.Combine()
    };
    
    #pragma warning disable IL2026
    #pragma warning disable IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
    private static IJsonTypeInfoResolver CreateDefaultTypeResolver()
        => new DefaultJsonTypeInfoResolver();
    #pragma warning restore IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
    #pragma warning restore IL2026
}