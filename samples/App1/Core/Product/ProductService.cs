using Polyrific.Project.Core;
using Polyrific.Project.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Product
{
    public class ProductService : IProductService
    {
        private readonly IRepository<ProductEntity> _productRepository;

        public ProductService(IRepository<ProductEntity> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> SaveProduct(ProductEntity product, bool createIfNotExist = true)
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

        public async Task<IEnumerable<ProductEntity>> GetProducts(string name)
        {
            var spec = new Specification<ProductEntity>(e => e.Name.Contains(name));
            var items = await _productRepository.GetBySpec(spec);

            return items;
        }

        public Task<ProductEntity> GetProduct(int id)
        {
            return _productRepository.GetById(id);
        }

        public async Task<(IEnumerable<ProductEntity> entities, int total)> GetProducts(int page, int size)
        {
            var spec = new Specification<ProductEntity>(e => true, e => e.Name, false, (page - 1) * size, size);

            var entities = await _productRepository.GetBySpec(spec);
            var total = await _productRepository.CountBySpec(spec);

            return (entities, total);
        }
    }
}
