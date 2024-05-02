
using OnlineShop.Core;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Schemas;
using OnlineShop.UseCases.Category.Crud;

namespace OnlineShop.WebApi.Controllers
{
    public class CategoryCtrl
    {
        private readonly CrudCategoryFlow workflow;
        public CategoryCtrl(IDataContext ctx)
        {
            workflow = new CrudCategoryFlow(ctx);
        }
        public IResult Get()
        {
            Response response = workflow.List();
            return Results.Ok(response);
        }
    }
}
