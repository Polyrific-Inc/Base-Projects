using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SampleMvc.Core.Services
{
    public interface IProductService
    {
        Task<int> AddProduct(string name);
    }
}
