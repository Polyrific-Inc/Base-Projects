using System.Linq;
using AutoMapper;
using SampleAngular.Core.Entities;

namespace SampleAngular.Data.Identity
{
    public class IdentityAutoMapperProfile : Profile
    {
        public IdentityAutoMapperProfile()
        {
            CreateMap<User, ApplicationUser>()
              .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
              .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
	             .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
	             .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
	             .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
	             .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
	             .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
	             .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
	             .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
	             .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
	             .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
	             .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore());
            CreateMap<ApplicationUser, User>();
        }
    }
}

