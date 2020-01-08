///OpenCatapultModelId:109
using AutoMapper;
using SampleMvc.Core.Entities;
using SampleMvc.Admin.Models;

namespace SampleMvc.Admin.AutoMapperProfiles
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
