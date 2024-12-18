﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Store.Application.Interfaces.Services;
using Store.Domain.Entities.Interfaces;
using Store.WebApplicationMVC.ViewModel;

namespace Store.WebApplicationMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly IUserContext _userContext;
        public CartController(ICartService cartService, IProductService productService, IUserContext userContext)
        {
            _cartService = cartService;
            _productService = productService;
            _userContext = userContext;
        }
        public IActionResult Index()
        {
            if (TempData["CartError"] != null)
            {
                this.ModelState.AddModelError("", TempData["CartError"].ToString());
                TempData["CartError"] = null;
            }

            CartViewModel cartViewModel = new CartViewModel()
            {
                TotalPrice = _cartService.ComputeTotalValue(),
                ProductDetails = _cartService.Lines,
                IsUserLoggedIn = _userContext.IsAuthenticated && _userContext.UserId != null
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
            var product = _cartService.Lines.Select(l => l.Product).First(p => p.Id == productId);
            _cartService.RemoveItem(product);
            return RedirectToAction("Index");
        }
    }
}
