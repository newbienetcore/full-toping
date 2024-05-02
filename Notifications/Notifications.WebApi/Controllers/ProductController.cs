using System.Collections;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Notifications.Application.Products.Commands.Crawler;
using Notifications.Application.Products.Models;
using Shared.Crawler.MessageContacts;

namespace Notifications.WebApi.Controllers;

public class ProductController : ApiControllerBase
{
    private readonly IRequestClient<CrawlerDataMessage> _client;

    public ProductController(IRequestClient<CrawlerDataMessage> client)
    {
        _client = client;
    }
    [HttpPost]
    [Route("api/crawler")]
    public async Task<IActionResult> CrawlerAsync([FromBody]CrawlerDataMessage message, CancellationToken cancellationToken)
    {
        var response = await _client.GetResponse<CrawlerProductCommandResponse>(message, cancellationToken);
        return Ok(response.Message);
    }
    
}