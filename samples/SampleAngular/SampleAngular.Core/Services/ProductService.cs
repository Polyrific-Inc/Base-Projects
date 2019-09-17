using Polyrific.Project.Core;
using SampleAngular.Core.Entities;
using SampleAngular.Core.Specification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleAngular.Core.Services
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

        public Task<IEnumerable<Product>> GetProducts()
        {
            return _productRepository.GetBySpec(new ProductSpecification());
        }
    }
}
