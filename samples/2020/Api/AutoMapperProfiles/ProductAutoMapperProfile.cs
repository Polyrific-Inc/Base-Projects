using Api.Dto;
using AutoMapper;
using Core.Entities;

namespace Api.AutoMapperProfiles
{
    public class ProductAutoMapperProfile : Profile
    {
        public ProductAutoMapperProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<NewProductDto, Product>();
            CreateMap<UpdatedProductDto, Product>();
        }
    }
}