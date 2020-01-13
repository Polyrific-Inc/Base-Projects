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

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            var entities = await _productService.GetProducts();

            return _mapper.Map<IEnumerable<ProductDto>>(entities);
        }

        [HttpGet("{id}")]
        public async Task<ProductDto> GetById(int id)
        {
            var entity = await _productService.GetProduct(id);

            return _mapper.Map<ProductDto>(entity);
        }

        [HttpGet("name/{name}")]
        public async Task<IEnumerable<ProductDto>> GetByName(string name)
        {
            var entities = await _productService.GetProducts(name);

            return _mapper.Map<IEnumerable<ProductDto>>(entities);
        }

        [HttpPut("{id}")]
        public async Task Update(int id, Product product)
        {
            await _productService.EditProduct(product);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _productService.DeleteProduct(id);
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