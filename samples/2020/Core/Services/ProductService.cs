using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.Extensions.Logging;
using Polyrific.Project.Core;
using Polyrific.Project.Core.Exceptions;

namespace Core.Services
{
    public class ProductService : BaseService<Product>, IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository, ILogger<ProductService> logger)
            : base(productRepository, logger)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetProducts(string name)
        {
            var spec = new Specification<Product>(e => e.Name.Contains(name));
            var items = await _productRepository.GetBySpec(spec);

            return items;
        }

        public async Task<Paging<Product>> GetPageDataWithFilterOperation(int? page = null,
            int? pageSize = null,
            string orderBy = null,
            string filter = null,
            Op operation = Op.Contains,
            bool @descending = false)
        {
            if (page < 1)
                page = 1;

            int? skip = (page - 1) * pageSize;

            // Sorting
            Expression<Func<Product, object>> _orderBy = ExpressionBuilder.GetSortExpression<Product>(orderBy);

            // Filter
            Expression<Func<Product, bool>> criteria = u => true;

            if (!string.IsNullOrEmpty(filter))
            {
                var filters = ExpressionBuilder.BuildFilter(filter, operation);

                criteria = ExpressionBuilder.GetExpression<Product>(filters);
            }

            var spec = new Specification<Product>(criteria, _orderBy, descending, skip, pageSize);

            try
            {
                var items = await Repository.GetBySpec(spec);
                var total = await Repository.CountBySpec(spec);

                return new Paging<Product>(items, total, page, pageSize);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to get {pageSize} Product on page {page}", pageSize, page);

                return new Paging<Product>();
            }
        }
    }
}