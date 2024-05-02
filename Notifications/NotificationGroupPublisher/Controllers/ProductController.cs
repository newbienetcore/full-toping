using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Crawler.MessageContacts;


namespace NotificationGroupPublisher.Controllers;

// public class ProductController : ApiControllerBase
// {
//     private readonly IRequestClient<CrawlerDataMessage> _client;
//         
//     public ProductController(IRequestClient<CrawlerDataMessage> client)
//     {
//         _client = client;
//     }
//
//     [HttpPost]
//     [Route("api/crawler")]
//     public async Task<IActionResult> CrawlerAsync([FromBody] CrawlerDataMessage message, CancellationToken cancellationToken = default)
//     {
//         var response = await _client.GetResponse<OrderStatusResult>(new { orderId }, cancellationToken);
//         return Ok(response.Message);
//     }
//
// }