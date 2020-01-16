using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Services
{
    public interface IProductService
    {
        Task<int> SaveProduct(Product product, bool createIfNotExist = true);
        Task DeleteProduct(int id);
        Task<IEnumerable<Product>> GetProducts(string name);
        Task<(IEnumerable<Product> entities, int total)> GetProducts(int page, int size);
        Task<Product> GetProduct(int id);
    }
}