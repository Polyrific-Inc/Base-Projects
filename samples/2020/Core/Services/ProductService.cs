using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Polyrific.Project.Core;
using Polyrific.Project.Core.Exceptions;

namespace Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> SaveProduct(Product product, bool createIfNotExist = true)
        {
            if (product.Id > 0)
            {
                var entity = await _productRepository.GetById(product.Id);
                if (entity != null)
                {
                    entity.Name = product.Name;
                    await _productRepository.Update(entity);

                    return entity.Id;
                }

                if (!createIfNotExist)
                    throw new NotExistEntityException(product.Id);
            }
            
            var newId = await _productRepository.Create(product);

            return newId;
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

        public Task<Product> GetProduct(int id)
        {
            return _productRepository.GetById(id);
        }

        public async Task<(IEnumerable<Product> entities, int total)> GetProducts(int page, int size)
        {
            var spec = new Specification<Product>(e => true, e => e.Name, false, (page - 1) * size, size);
            
            var entities = await _productRepository.GetBySpec(spec);
            var total = await _productRepository.CountBySpec(spec);

            return (entities, total);
        }
    }
}