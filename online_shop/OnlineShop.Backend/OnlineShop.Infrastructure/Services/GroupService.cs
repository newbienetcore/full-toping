using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Schemas.Base;
namespace OnlineShop.Infrastructure.Services
{

    public class GroupService : IGroup
    {

        private readonly DataContext context;
        public GroupService(DataContext _ctx)  
        {
            context = _ctx;
        }
         
	}
}
