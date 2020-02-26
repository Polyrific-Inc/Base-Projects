using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Product
{
    public interface IProductService
    {
        Task<int> SaveProduct(ProductEntity product, bool createIfNotExist = true);
        Task DeleteProduct(int id);
        Task<IEnumerable<ProductEntity>> GetProducts(string name);
        Task<(IEnumerable<ProductEntity> entities, int total)> GetProducts(int page, int size);
        Task<ProductEntity> GetProduct(int id);
    }
}
