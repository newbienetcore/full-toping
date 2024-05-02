using OnlineShop.Core.Interfaces;

namespace OnlineShop.Infrastructure.Services
{ 

    public class CategoryService : ICategory
    {

        private readonly DataContext context;

        public CategoryService(DataContext _ctx)
        {
            this.context = _ctx;
        }
    }    
}
