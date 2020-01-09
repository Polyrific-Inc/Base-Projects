using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dto;
using AutoMapper;
using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
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

        [HttpGet("{name}")]
        public async Task<IEnumerable<ProductDto>> GetByName(string name)
        {
            var entities = await _productService.GetProducts(name);

            return _mapper.Map<IEnumerable<ProductDto>>(entities);
        }

        [HttpPost]
        public async Task<ActionResult> Post(NewProductDto newProduct)
        {
            var entity = _mapper.Map<Product>(newProduct);
            var newId = await _productService.AddProduct(entity);

            return new OkObjectResult(newId);
        }
    }
}