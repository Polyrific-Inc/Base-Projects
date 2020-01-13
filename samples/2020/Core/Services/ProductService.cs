using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Polyrific.Project.Core;

namespace Core.Services
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

        public async Task EditProduct(Product product)
        {
            var entity = await _productRepository.GetById(product.Id);
            entity.Name = product.Name;
            await _productRepository.Update(entity);
        }

        public Task DeleteProduct(int id)
        {
            return _productRepository.Delete(id);
        }

        public async Task<IEnumerable<Product>> GetProducts(string name)
        {
            var spec = new Specification<Product>(e => e.Name.Contains(name));
            var items = await _productRepository.GetBySpec(spec);

            return items;
        }

        public async Task<Product> GetProduct(int id)
        {
            return await _productRepository.GetById(id);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var spec = new Specification<Product>(e => true);
            var items = await _productRepository.GetBySpec(spec);

            return items;
        }
    }
}