using OnlineShop.Core;
using OnlineShop.Core.Schemas.Base;
using OnlineShop.UseCases.User.Crud;
using OnlineShop.Application.UseCases.User.Crud.Presenter;
using AutoMapper;
using OnlineShop.Core.Interfaces;
using static OnlineShop.Utils.CtrlUtil;

namespace OnlineShop.WebApi.Controllers
{
    public class UserCtrl
    {
        private readonly IMapper _mapper;
        private readonly CrudUserFlow workflow;
        public UserCtrl(IMapper mapper, IDataContext ctx)
        {
            _mapper = mapper;
            workflow = new CrudUserFlow(ctx);
        }

        public async Task<IResult> GetAsync()
        {
            Response response = await workflow.ListAsync();
            return Results.Ok(response);
        }
        public async Task<IResult> CreateAsync(UserSchema model)
        {
            //UserSchema user = _mapper.Map<UserSchema>(model);
            Response response = await workflow.CreateAsync(model);
            return Results.Ok(response);
        }
        public async Task<IResult> UpdateAsync(UpdateUserPresenter model)
        {
            UserSchema user = _mapper.Map<UserSchema>(model);
            Response response = await workflow.UpdateAsync(user);
            return Results.Ok(response);
        }
        public async Task<IResult> DeletesAsync(int[] ids)
        {
            Response response = await workflow.DeletesAsync(ids);
            return Results.Ok(response);
        }
    }
}
