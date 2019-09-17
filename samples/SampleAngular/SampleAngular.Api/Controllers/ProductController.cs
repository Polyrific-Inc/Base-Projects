using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleAngular.Core.Entities;
using SampleAngular.Core.Services;
using SampleAngular.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleAngular.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> Get()
        {
            var products = await _productService.GetProducts();
            return _mapper.Map<List<ProductDto>>(products);
        }

        [HttpPost]
        public async Task Post([FromBody] ProductDto product)
        {
            var newProduct = _mapper.Map<Product>(product);
            await _productService.AddProduct(newProduct);
        }
    }
}