﻿using SampleAngular.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleAngular.Core.Services
{
    public interface IProductService
    {
        Task<int> AddProduct(string name);
        Task<IEnumerable<Product>> GetProducts();
    }
}
