using Microsoft.AspNetCore.Identity;
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
        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid productId)
        {
            var product = await _productService.GetByIdAsync(productId);
            _cartService.AddItem(product, 1);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(Guid productId)
        {
            var product = await _productService.GetByIdAsync(productId);
            _cartService.RemoveItem(product);
            return RedirectToAction("Index");
        }
    }
}
