using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Product
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public CreateModel(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [BindProperty]
        public ProductViewModel Item { get; set; }

        [TempData]
        public string InfoMessage { get; set; }

        public void OnGet()
        {
            Item = new ProductViewModel();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var entity = _mapper.Map<ProductEntity>(Item);

            try
            {
                _ = await _productService.SaveProduct(entity);

                InfoMessage = $"Product \"{entity.Name}\" has been created successfully";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CreateError", $"Failed to create product: {ex.Message}");
                return Page();
            }

            return RedirectToPage("Index");
        }

        public IActionResult OnPostCancel()
        {
            return RedirectToPage("Index");
        }
    }
}