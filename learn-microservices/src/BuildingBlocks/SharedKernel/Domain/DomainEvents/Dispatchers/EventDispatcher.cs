using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SharedKernel.Domain;
using SharedKernel.Libraries;
using SharedKernel.Log;
using SharedKernel.MySQL;
using SharedKernel.RabbitMQ;

namespace SharedKernel.DomainEvents
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IMediator _mediator;
        private readonly IRabbitMqClientBase _rabbitMqClient;
        private readonly IServiceProvider _provider;

        public EventDispatcher(IServiceProvider provider)
        {
            _provider = provider.CreateScope().ServiceProvider;
            _mediator = _provider.GetRequiredService<IMediator>();
            _rabbitMqClient = _provider.GetRequiredService<IRabbitMqClientBase>();
        }

        public async Task FireEvent<T>(T @event, CancellationToken cancellationToken = default) where T : DomainEvent
        {
            
        }

        public async Task FireEvent<T>(List<T> events, CancellationToken cancellationToken = default) where T : DomainEvent
        {
           
        }

        private async Task Publish<T>(T @event, CancellationToken cancellationToken = default) where T : DomainEvent
        {
           
        }

       
    }
}
