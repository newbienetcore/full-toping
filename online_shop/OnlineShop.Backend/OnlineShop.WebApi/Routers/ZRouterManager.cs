using AutoMapper;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Models;
using System.Reflection;

namespace OnlineShop.WebApi.Routers
{
    public class ZRouterManager
    {
        private readonly byte[] secretKey;
        private readonly IMapper mapper;
        public ZRouterManager(byte[] _secretKey, IMapper _mapper)
        {
            secretKey = _secretKey;
            mapper = _mapper;
        }
        public List<RouterModel> Get(IDataContext ctx)
        {
            List<RouterModel> routers = new List<RouterModel>();

            var routerInstances = CreateRouterInstances(ctx, mapper, secretKey);

            foreach (var routerInstance in routerInstances)
            {
                var routerMethod = routerInstance.GetType().GetMethod("Get");
                if (routerMethod != null)
                {
                    var routerModels = routerMethod.Invoke(routerInstance, null) as IEnumerable<RouterModel>;
                    if (routerModels != null)
                    {
                        routers.AddRange(routerModels);
                    }
                }
            }
            return routers;
        }

        private IEnumerable<object> CreateRouterInstances(IDataContext ctx, IMapper mapper, byte[] secretKey)
        {
            var routerInterface = typeof(IRoute);
            var routerClasses = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => routerInterface.IsAssignableFrom(type) && type != routerInterface)
                .ToList();
            var routerInstances = new List<object>();
            foreach (var routerClass in routerClasses)
            {
                var types = new[]
                {
                    new[] { typeof(IDataContext) },
                    new[] { typeof(IDataContext), typeof(IMapper) },
                    new[] { typeof(IDataContext), typeof(IMapper), typeof(byte[]) }
                };

                var paramInstances = new[]
{
                    new object[] { ctx },
                    new object[] { ctx, mapper },
                    new object[] { ctx, mapper, secretKey  }
                };

                int index = 0;
                for (int i = 0; i < types.Length; i++)
                {
                    var c = routerClass.GetConstructor(types[i]);
                    if (c != null)
                    {
                        index = i;
                        break;
                    }
                }

                var routerInstance = Activator.CreateInstance(routerClass, paramInstances[index]);
                routerInstances.Add(routerInstance);
            }
            return routerInstances;
        }
    }
}
