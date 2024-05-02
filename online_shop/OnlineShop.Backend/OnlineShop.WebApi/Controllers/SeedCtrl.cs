using AutoMapper;
using OnlineShop.Core;
using OnlineShop.Core.Interfaces;
using OnlineShop.UseCases.SyncAllPerm;
using OnlineShop.WebApi.Routers;

namespace OnlineShop.WebApi.Controllers
{
    public class SeedCtrl
    {
        private readonly SeedFlow workflow;
        private readonly IDataContext context;
        private readonly byte[] secretKey;
        private readonly IMapper mapper;
        public SeedCtrl(IDataContext ctx, byte[] _secretKey, IMapper _mapper)
        {
            secretKey = _secretKey;
            context = ctx;
            mapper = _mapper;
            workflow = new SeedFlow(ctx);
        }
        public async Task<IResult> SyncAllPerm()
        {
            var routers = new ZRouterManager(secretKey, mapper).Get(context);
            Response response = await workflow.Seed(routers);
            return Results.Ok(response);
        }
    }
}
