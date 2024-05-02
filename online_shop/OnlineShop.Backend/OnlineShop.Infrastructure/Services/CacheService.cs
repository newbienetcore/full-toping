using Microsoft.Extensions.Caching.Memory;
using OnlineShop.Core.Interfaces;

namespace OnlineShop.Infrastructure.Services
{
    public class CacheService<TEntity> : ICacheService<TEntity>
       where TEntity : class
    {
        private readonly IMemoryCache _memoryCache;
        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public async void Set(string cacheKey, List<TEntity> datas)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromDays(1));
            _memoryCache.Set(cacheKey, datas, cacheEntryOptions);
        }



        public List<TEntity> Get(string cacheKey)
        {
            _memoryCache.TryGetValue(cacheKey, out IEnumerable<TEntity> dataCached);
            return dataCached == null ? null : dataCached.ToList();
        }
    }
}
