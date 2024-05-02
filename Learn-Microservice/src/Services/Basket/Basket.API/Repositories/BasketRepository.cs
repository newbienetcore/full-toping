using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using ILogger = Serilog.ILogger;

namespace Basket.API.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _distributedCache;
    private readonly ISerializeService _serializeService;
    private readonly ILogger _logger;
    
    public BasketRepository(IDistributedCache distributedCache,
        ISerializeService serializeService,
        ILogger logger)
    {
        _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        _serializeService = serializeService ?? throw new ArgumentNullException(nameof(serializeService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<Cart?> GetCartByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        _logger.Information($"BEGIN: GetCartByUserNameAsync: {userName}");
        var basket = await _distributedCache.GetStringAsync(userName, cancellationToken);
        _logger.Information($"END: GetCartByUserNameAsync: {userName}");
        
        return string.IsNullOrEmpty(basket) ? default! : _serializeService.Deserialize<Cart>(basket);
    }

    public async Task<Cart> UpdateBasketAsync(Cart cart, DistributedCacheEntryOptions options = null, CancellationToken cancellationToken = default)
    {
        _logger.Information($"BEGIN: UpdateBasketAsync for: {cart.UserName}");
        if (options is not null)
        {
             await _distributedCache.SetStringAsync(cart.UserName, _serializeService.Serialize(cart), options, cancellationToken);
        }
        else
        {
            await _distributedCache.SetStringAsync(cart.UserName, _serializeService.Serialize(cart), cancellationToken);
        }
        _logger.Information($"END: UpdateBasketAsync for: {cart.UserName}");
        
        return await GetCartByUserNameAsync(cart.UserName, cancellationToken);
    }

    public async Task<bool> DeleteBasketFromUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.Information($"BEGIN: DeleteBasketFromUserNameAsync: {userName}");
            await _distributedCache.RemoveAsync(userName, cancellationToken);
            _logger.Information($"END: DeleteBasketFromUserNameAsync: {userName}");
            
            return true;
        }
        catch (Exception exception)
        {
            _logger.Error("ERROR: DeleteBasketFromUserNameAsync: " + exception.Message);
            throw;
        }
    }
}