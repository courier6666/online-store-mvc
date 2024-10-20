using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.DataTransferObjects;
using Store.Application.Interfaces.Services;
using Store.Domain.Entities.Model;
using Store.WebApplicationMVC.ViewModel;

namespace Store.WebApplicationMVC.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class ProductAdminController : Controller
    {
        private readonly IProductService _productService;
        public ProductAdminController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create(string returnUrl = "/")
        {
            return View(new ProductViewModel()
            {
                ReturnUrl = returnUrl,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductViewModel product)
        {
            await _productService.AddAsync(new ProductDto()
            {
                Name = product.Name,
                Category = product.Category,
                Description = product.Description,
                Price = product.Price,
            });

            return Redirect(product.ReturnUrl ?? "/");
        }
        
        public async Task<IActionResult> Edit(Guid productId, string returnUrl = "/")
        {
            var product = await _productService.GetByIdAsync(productId);
            if (product is null) return NotFound();

            return View(new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                Description = product.Description,
                Price = product.Price,
                ReturnUrl = returnUrl,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm]ProductViewModel product)
        {
            await _productService.UpdateAsync(new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                Description = product.Description,
                Price = product.Price,
            });

            return Redirect(product.ReturnUrl ?? "/");
        }
        public async Task<IActionResult> Delete(Guid productId, string returnUrl = "/")
        {
            var product = await _productService.GetByIdAsync(productId);
            if (product is null) return NotFound();

            return View(new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                Description = product.Description,
                Price = product.Price,
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] ProductViewModel product)
        {
            await _productService.DeleteAsync(product.Id.Value);
            return Redirect(product.ReturnUrl);
        }
    }
}
