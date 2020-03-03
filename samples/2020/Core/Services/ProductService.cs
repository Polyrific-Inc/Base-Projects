using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.Extensions.Logging;
using Polyrific.Project.Core;
using Polyrific.Project.Core.Exceptions;

namespace Core.Services
{
    public class ProductService : BaseService<Product>, IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository, ILogger<ProductService> logger)
            : base(productRepository, logger)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetProducts(string name)
        {
            var spec = new Specification<Product>(e => e.Name.Contains(name));
            var items = await _productRepository.GetBySpec(spec);

            return items;
        }
    }
}