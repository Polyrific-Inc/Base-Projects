using Polyrific.Project.Core;
using SampleMvc.Core.Entities;
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

        public Task<int> AddProduct(string name)
        {
            var newProduct = new Product
            {
                Name = name
            };

            return _productRepository.Create(newProduct);
        }
    }
}
