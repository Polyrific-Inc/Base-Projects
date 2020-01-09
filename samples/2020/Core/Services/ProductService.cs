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

        public async Task<IEnumerable<Product>> GetProducts(string name)
        {
            var spec = new Specification<Product>(e => e.Name.Contains(name));
            var items = await _productRepository.GetBySpec(spec);

            return items;
        }
    }
}