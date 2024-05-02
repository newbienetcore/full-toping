

using OnlineShop.Core;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Schemas.Base;
using OnlineShop.Utils;

namespace OnlineShop.UseCases.Permission
{
    public class VerifyFlow
    {
        private readonly IPerm permService;
        private readonly ICacheService<PermSchema> cacheService;

        public VerifyFlow(ICacheService<PermSchema> _cacheService, IPerm _permService)
        {
            cacheService = _cacheService;
            permService = _permService;
        }
        public Response Execute(string module, string action, int userId)
        {
            var perms = cacheService.Get(CacheKey.PERM_CACHE_KEY + userId);
            if (perms == null || perms.Count == 0)
            {
                perms = permService.GetPerms(userId);
                cacheService.Set(CacheKey.PERM_CACHE_KEY + userId, perms);
            }
            PermSchema perm = perms.Find(x => x.Module.ToLower().Equals(module) && x.Action.Equals(action));
            return new Response(Message.SUCCESS, perm);
        }
    }
}
