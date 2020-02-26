using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Web.Product
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IProductService productService, IMapper mapper, ILogger<IndexModel> logger)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }

        public Paging<ProductViewModel> Items { get; private set; }

        [TempData]
        public string InfoMessage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGet(int? page, int? size)
        {
            var (entities, total) = await _productService.GetProducts(1, 20);

            Items = new Paging<ProductViewModel>
            {
                Items = _mapper.Map<IEnumerable<ProductViewModel>>(entities),
                TotalCount = total,
                Page = page ?? 1,
                PageSize = size ?? 20
            };

            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int id, string name)
        {
            try
            {
                await _productService.DeleteProduct(id);

                _logger.LogInformation("Product {productId} has been deleted successfully", id);
                InfoMessage = $"Product \"{name}\" has been deleted successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete product {productId}", id);
                ErrorMessage = $"Failed to delete product \"{name}\". Please check log.";
            }

            return RedirectToPage();
        }
    }
}