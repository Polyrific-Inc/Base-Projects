///OpenCatapultModelId:111
using AutoMapper;
using SampleMvc.Core.Entities;
using SampleMvc.Admin.Models;

namespace SampleMvc.Admin.AutoMapperProfiles
{
    public class ProductAutoMapperProfile : Profile
    {
        public ProductAutoMapperProfile()
        {
            CreateMap<Product, ProductViewModel>();

            CreateMap<ProductViewModel, Product>();
        }
    }
}
