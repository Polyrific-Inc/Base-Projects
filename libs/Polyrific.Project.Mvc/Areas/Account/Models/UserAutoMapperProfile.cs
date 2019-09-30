using AutoMapper;

namespace Polyrific.Project.Mvc.Areas.Account.Models
{
    public class UserAutoMapperProfile : Profile
    {
        public UserAutoMapperProfile()
        {
            CreateMap<Core.Entities.User, UserViewModel>();

            CreateMap<UserViewModel, Core.Entities.User>();
        }
    }
}
