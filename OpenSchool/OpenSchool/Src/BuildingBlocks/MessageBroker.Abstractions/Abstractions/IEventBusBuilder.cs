using Microsoft.Extensions.DependencyInjection;

namespace MessageBroker.Abstractions;

public interface IEventBusBuilder
{
    public IServiceCollection Services { get; }
}