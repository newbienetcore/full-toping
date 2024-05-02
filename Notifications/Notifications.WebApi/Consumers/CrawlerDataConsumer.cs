using MassTransit;
using MediatR;
using Notifications.Application.Products.Commands.Crawler;
using Notifications.Application.Products.Models;
using Shared.Crawler.MessageContacts;

namespace Notifications.WebApi.Consumers;

public class CrawlerDataConsumer : IConsumer<CrawlerDataMessage>
{
    private readonly IMediator _mediator;
    
    public CrawlerDataConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Consume(ConsumeContext<CrawlerDataMessage> context)
    {
        var message = context.Message;

        var products = await _mediator.Send(new CrawlerProductCommand()
        {
            Source = message.Source
        });
        
        await context.RespondAsync(products);
    }
}