using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Services
{
    public interface IProductService
    {
        Task<int> AddProduct(Product newProduct);
        Task<IEnumerable<Product>> GetProducts(string name);
    }
}