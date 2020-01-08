using AutoMapper;
using SampleAngular.Core.Entities;
using SampleAngular.Dto;

namespace SampleAngular.Api.AutoMapperProfiles
{
    public class ProductAutoMapperProfile : Profile
    {
        public ProductAutoMapperProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}
