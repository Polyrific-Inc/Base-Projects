using AutoMapper;
using Core.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Product
{
    public class ProductAutoMapperProfile : Profile
    {
        public ProductAutoMapperProfile()
        {
            CreateMap<ProductEntity, ProductViewModel>().ReverseMap();
        }
    }
}
