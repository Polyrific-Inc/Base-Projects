using SampleAngular.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleAngular.Core.Services
{
    public interface IProductService
    {
        Task<int> AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int productId);
        Task<Product> GetProduct(int productId);
        Task<IEnumerable<Product>> GetProducts();
    }
}
