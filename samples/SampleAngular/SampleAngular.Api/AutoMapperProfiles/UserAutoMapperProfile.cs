using AutoMapper;
using SampleAngular.Core.Entities;
using SampleAngular.Dto;

namespace SampleAngular.Api.AutoMapperProfiles
{
    public class UserAutoMapperProfile : Profile
    {
        public UserAutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<RegisterUserDto, User>();
            CreateMap<UpdateUserDto, User>();
        }
    }
}
