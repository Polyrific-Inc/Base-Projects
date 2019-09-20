using AutoMapper;
using Polyrific.Project.Core.Entities;

namespace Polyrific.Project.Mvc.Areas.Account.Models
{
    public class UserAutoMapperProfile : Profile
    {
        public UserAutoMapperProfile()
        {
            CreateMap<User, UserViewModel>();

            CreateMap<UserViewModel, User>();
        }
    }
}
