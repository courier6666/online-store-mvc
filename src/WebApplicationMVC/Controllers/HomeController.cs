using Microsoft.AspNetCore.Mvc;
using Store.Application.Interfaces.Services;
using Store.Application.Queries;
using Store.Domain.Entities;
using Store.WebApplicationMVC.Models;
using Store.WebApplicationMVC.ViewModel;

namespace Store.WebApplicationMVC.Controllers
{
    public class HomeController : Controller
    {
        private IProductService _productService;
        public HomeController(IProductService productService)
        {
            _productService = productService;
        }
        int PageSize = 20;
        public async Task<IActionResult> Index(string? category,
            decimal? minPrice,
            decimal? maxPrice,
            string? productName = null,
            int page = 1,
            string? order = null)
        {
            if (string.IsNullOrEmpty(category)) category = null;
            var products = await _productService.GetPagedProductsAsync(new ProductsPageQuery()
            {
                Category = category,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Page = page,
                PageSize = PageSize,
                SortOrder = order,
                ProductName = productName
            });

            ProductHomeViewModel productHomeViewModel = new ProductHomeViewModel()
            {
                Products = products,
                PagingInfo = new PagingInfo()
                {
                    HasNextPage = products.HasNextPage,
                    HasPreviousPage = products.HasPreviousPage,
                    Page = page,
                    PageSize = PageSize,
                    TotalCount = products.TotalCount,
                    TotalPages = products.TotalPages,
                },
                Category = category,
                MaxPrice = maxPrice,
                MinPrice = minPrice,
                AllCategories = await _productService.GetAllCategoriesAsync(),
                CurrentValueSortedBy = order,
                ProductName = productName
            };

            return View(productHomeViewModel);
        }
        [HttpPost]
        public IActionResult ApplyCriteriaSearchForm(string? category,
            decimal? minPrice,
            decimal? maxPrice,
            string? productName = null)
        {
            return RedirectToAction("Index", new { category, minPrice, maxPrice, productName });
        }
        [HttpPost]
        public IActionResult FindByProductName(string productName)
        {
            if (string.IsNullOrEmpty(productName))
                return RedirectToAction("Index");
            return RedirectToAction("Index", new { productName });
        }
        [HttpPost]
        public async Task<IActionResult> Index([FromForm] ProductHomeViewModel productHomeViewModel)
        {
            return RedirectToAction("Index", new { category = productHomeViewModel.Category, minPrice = productHomeViewModel.MinPrice, maxPrice = productHomeViewModel.MaxPrice, page = productHomeViewModel.PagingInfo.Page });
        }
    }
}
