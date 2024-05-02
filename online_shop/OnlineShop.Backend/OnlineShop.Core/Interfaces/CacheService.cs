using Microsoft.Extensions.Caching.Memory;

namespace OnlineShop.Core.Interfaces
{
    public interface ICacheService<T> where T : class
    {
        void Set(string cacheKey, List<T> datas);
        List<T> Get(string cacheKey);
    }
}
