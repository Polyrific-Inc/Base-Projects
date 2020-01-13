using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Services
{
    public interface IProductService
    {
        Task<int> AddProduct(Product newProduct);
        Task EditProduct(Product product);
        Task DeleteProduct(int id);
        Task<IEnumerable<Product>> GetProducts(string name);
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(int id);
    }
}