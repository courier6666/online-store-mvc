using Microsoft.AspNetCore.Mvc;
using Store.Application.Interfaces.Services;

namespace Store.WebApplicationMVC.Components
{
    [ViewComponent]
    public class NavigationBarViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        public NavigationBarViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _productService.GetAllCategoriesAsync());
        }
    }
}
