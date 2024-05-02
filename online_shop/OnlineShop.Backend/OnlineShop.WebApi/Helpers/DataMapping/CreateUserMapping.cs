using AutoMapper;
using OnlineShop.Application.UseCases.User.Crud.Presenter;
using OnlineShop.Core.Schemas.Base;

namespace OnlineShop.WebApi.Helpers.DataMapping
{
    public class CreateUserMapping : Profile
    {
        public CreateUserMapping()
        {
            // Default mapping when property names are same
            CreateMap<CreateUserPresenter, UserSchema>();
            CreateMap<UpdateUserPresenter, UserSchema>();

            // Mapping when property names are different
            //CreateMap<User, UserViewModel>()
            //    .ForMember(dest =>
            //    dest.FName,
            //    opt => opt.MapFrom(src => src.FirstName))
            //    .ForMember(dest =>
            //    dest.LName,
            //    opt => opt.MapFrom(src => src.LastName));
        }
    }

}
