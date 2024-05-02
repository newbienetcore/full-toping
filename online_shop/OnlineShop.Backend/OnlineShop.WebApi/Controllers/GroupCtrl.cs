using OnlineShop.Core; 
using OnlineShop.UseCases.User.Crud;
using OnlineShop.Core.Interfaces;

namespace OnlineShop.WebApi.Controllers
{
    public class GroupCtrl
    {
        private readonly CrudUserFlow workflow;
        public GroupCtrl(IDataContext ctx)
        {
            workflow = new CrudUserFlow(ctx);
        }
        public async Task<IResult> GetAsync()
        {
            Response response = await workflow.ListAsync();
            return Results.Ok(response);
        }
    }
}
