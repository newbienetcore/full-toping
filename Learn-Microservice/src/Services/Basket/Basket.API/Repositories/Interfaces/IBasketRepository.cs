using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Repositories.Interfaces;

public interface IBasketRepository
{
    Task<Cart?> GetCartByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<Cart> UpdateBasketAsync(Cart cart, DistributedCacheEntryOptions options, CancellationToken cancellationToken = default);
    Task<bool> DeleteBasketFromUserNameAsync(string userName, CancellationToken cancellationToken = default);
}