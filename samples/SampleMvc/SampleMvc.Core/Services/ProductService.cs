using Polyrific.Project.Core;
using SampleMvc.Core.Entities;
using SampleMvc.Core.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleMvc.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<int> AddProduct(Product newProduct)
        {
            return _productRepository.Create(newProduct);
        }

        public Task DeleteProduct(int productId)
        {
            return _productRepository.Delete(productId);
        }

        public Task<Product> GetProduct(int productId)
        {
            return _productRepository.GetById(productId);
        }

        public Task<IEnumerable<Product>> GetProducts()
        {
            return _productRepository.GetBySpec(new ProductSpecification());
        }

        public async Task UpdateProduct(Product product)
        {
            var entity = await _productRepository.GetById(product.Id);
            entity.SetEntity(product);
            await _productRepository.Update(entity);
        }
    }
}
