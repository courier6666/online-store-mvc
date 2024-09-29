using Microsoft.AspNetCore.Mvc;
using Store.Application.Interfaces.Services;
using Store.WebApplicationMVC.ViewModel;

namespace Store.WebApplicationMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        public CartController(ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }
        public IActionResult Index()
        {
            CartViewModel cartViewModel = new CartViewModel()
            {
                TotalPrice = _cartService.ComputeTotalValue(),
                ProductDetails = _cartService.Lines,
            };
            return View(cartViewModel);
        }
    }
}
