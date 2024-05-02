using OnlineShop.Core.Interfaces;
using OnlineShop.Core;
using OnlineShop.Core.Schemas;
using OnlineShop.Utils;
using OnlineShop.UseCases.Category.Presenter;


namespace OnlineShop.UseCases.Category.Crud
{
	public class CrudCategoryFlow
	{
        private readonly IDataContext dbContext;
        public CrudCategoryFlow(IDataContext ctx)
        {
            dbContext = ctx;
        }
         
        public Response List()
        {
            var users = dbContext.Categories.ToList();
            return new Response(Message.SUCCESS, users);
        }
        public Response ListAndCount()
        {
            var result = dbContext.Categories.Select(u => new CategoryVM {
                Id = u.Id,
                Name = u.Name,
                Count = u.Products.Count,
            }).ToList();

            return new Response(Message.SUCCESS, result);
        }
        public Response ListProductById(int id)
        {
            var result = dbContext.Products.Where(u => u.CategoryID == id).ToList();
            return new Response(Message.SUCCESS, result);
        }
        
        public Response Create(CategorySchema category)
        {
            var result = dbContext.Categories.Add(category);
            return new Response(Message.SUCCESS, result);
        }

        public Response Update(int id,CategorySchema category)
        {
            var result = dbContext.Categories.Find(id);
            result.Name = category.Name;
            dbContext.SaveChanges();
            return new Response(Message.SUCCESS, result);
        }

        public Response Delete(int id)
        {
            var category = dbContext.Categories.Find(id);
            var result = dbContext.Categories.Remove(category);
            return new Response(Message.SUCCESS, result);
        }

    }
}
