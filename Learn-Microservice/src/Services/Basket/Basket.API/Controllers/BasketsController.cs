using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using EventBus.Messages.IntegrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Shared.SeedWork;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/baskets")]
public class BasketsController : ControllerBase
{ 
    private readonly IBasketRepository _basketRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public BasketsController(IBasketRepository basketRepository, IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _basketRepository = basketRepository;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("{username}")]
    [ProducesResponseType(typeof(ApiResult<>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ApiResult<Cart?>>> GetAsync([Required][FromRoute(Name = "username")]string userName)
    {
        var cart = await _basketRepository.GetCartByUserNameAsync(userName);
        if (cart is null) return new ApiResult<Cart>(new Cart());
        return new ApiResult<Cart>(cart);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResult<>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ApiResult<Cart>>> UpdateAsync([FromBody]Cart cart)
    {
        var option = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddHours(1))
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

        var result = await _basketRepository.UpdateBasketAsync(cart, option);
        return new ApiResult<Cart>(result);
    }

    [HttpDelete]
    [Route("{username}")]
    [ProducesResponseType(typeof(ApiResult<>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ApiResult<bool>>> DeleteAsync([Required] [FromRoute(Name = "username")] string userName)
    {
        var result = await _basketRepository.DeleteBasketFromUserNameAsync(userName);
        return new ApiResult<bool>();
    }

    [Route("checkout")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> CheckoutAsync([FromBody] BasketCheckoutEvent basketCheckout)
    {
        var basket = await _basketRepository.GetCartByUserNameAsync(basketCheckout.UserName);
        if (basket is null) return NotFound();
        
        // publish checkout event to Eventbus Message
        var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
        eventMessage.TotalPrice = basket.TotalPrice;
        await _publishEndpoint.Publish(eventMessage);
        
        // remove the basket
        await _basketRepository.DeleteBasketFromUserNameAsync(basketCheckout.UserName);
        
        return Accepted();
    }
}