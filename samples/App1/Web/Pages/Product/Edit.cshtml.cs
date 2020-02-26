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
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public EditModel(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [BindProperty]
        public ProductViewModel Item { get; set; }

        [TempData]
        public string InfoMessage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            var entity = await _productService.GetProduct(id);
            if (entity == null)
            {
                ErrorMessage = $"Product \"{id}\" was not found";
                return RedirectToPage("Index");
            }

            Item = _mapper.Map<ProductViewModel>(entity);

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var entity = await _productService.GetProduct(id);
            if (entity == null)
            {
                ErrorMessage = $"Product \"{id}\" was not found";
                return RedirectToPage("Index");
            }

            entity.Name = Item.Name;

            try
            {
                _ = await _productService.SaveProduct(entity, false);

                InfoMessage = $"Product \"{entity.Name}\" has been updated successfully";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("UpdateError", $"Failed to update product: {ex.Message}");
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