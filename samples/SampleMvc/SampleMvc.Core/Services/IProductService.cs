using SampleMvc.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleMvc.Core.Services
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
