using Microsoft.AspNetCore.Mvc;
using Store.Application.Interfaces.Services;
using Store.Application.Queries;
using Store.Domain.Entities;
using Store.Domain.Entities.Interfaces;
using Store.Domain.Entities.Model;
using Store.WebApplicationMVC.Models;
using Store.WebApplicationMVC.ViewModel;

namespace Store.WebApplicationMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IUserContext _userContext;
        public HomeController(IProductService productService, IUserService userService, IUserContext userContext)
        {
            _productService = productService;
            _userService = userService;
            _userContext = userContext;
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
                ProductName = productName,
                IsUserAdmin = _userContext.IsAuthenticated &&
                    _userContext.UserId != null &&
                    ((await _userService.GetRolesByUserIdAsync(_userContext.UserId.Value))?.Contains(Roles.Admin) ?? false)
            }; ;

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
