using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Polyrific.Project.Core;

namespace Core.Services
{
    public interface IProductService : IBaseService<Product>
    {
        Task<IEnumerable<Product>> GetProducts(string name);
        Task<Paging<Product>> GetPageDataWithFilterOperation(int? page = null,
            int? pageSize = null,
            string orderBy = null,
            string filter = null,
            Op operation = Op.Contains,
            bool @descending = false);
    }
}