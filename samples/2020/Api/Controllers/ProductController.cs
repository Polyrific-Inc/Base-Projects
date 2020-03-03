using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dto;
using AutoMapper;
using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Polyrific.Project.Core.Exceptions;

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
        public async Task<PageResult<ProductDto>> GetAll(int? page, int? size)
        {
            var result = await _productService.GetPageData(page ?? 1, size ?? 20);

            return new PageResult<ProductDto>{
                Items = _mapper.Map<IEnumerable<ProductDto>>(result.Items),
                TotalCount = result.TotalCount,
                Page = page ?? 1,
                PageSize = size ?? 20
            };
        }

        [HttpGet("{id}")]
        public async Task<ProductDto> GetById(int id)
        {
            var entity = await _productService.Get(id);

            return _mapper.Map<ProductDto>(entity);
        }

        [HttpGet("name/{name}")]
        public async Task<IEnumerable<ProductDto>> GetByName(string name)
        {
            var entities = await _productService.GetProducts(name);

            return _mapper.Map<IEnumerable<ProductDto>>(entities);
        }

        [HttpPost]
        public async Task<ActionResult> Post(NewProductDto newProduct)
        {
            var entity = _mapper.Map<Product>(newProduct);
            var result = await _productService.Save(entity);

            return Ok(result.Item.Id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, UpdatedProductDto product)
        {
            if (product.Id != id)
                return BadRequest("The Id parameter doesn't match with the object Id");

            try
            {
                var entity = _mapper.Map<Product>(product);
                await _productService.Save(entity, false);
            }
            catch (NotExistEntityException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _productService.Delete(id);

            return NoContent();
        }
    }
}