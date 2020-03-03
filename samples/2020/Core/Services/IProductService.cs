using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Polyrific.Project.Core;

namespace Core.Services
{
    public interface IProductService : IBaseService<Product>
    {
        Task<IEnumerable<Product>> GetProducts(string name);
    }
}