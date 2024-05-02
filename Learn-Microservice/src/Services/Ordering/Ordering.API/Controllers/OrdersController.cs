using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders.Commands.CreateOrder;
using Ordering.Application.Features.V1.Queries.GetOrders;

namespace Ordering.API.Controllers;

[Route("api/v1/orders")]
public class OrdersController : ApiControllerBase
{

    private static class RouteNames
    {
        public const string GetOrders = nameof(GetOrders);
        public const string CreateOrder = nameof(CreateOrder);
        public const string UpdateOrder = nameof(UpdateOrder);
        public const string DeleteOrder = nameof(DeleteOrder);
    }

    [HttpGet]
    [Route("{userName}", Name = RouteNames.GetOrders)]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersAsync([Required] string userName)
    {
        var query = new GetOrdersQuery(userName);
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    
    
}