using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Interfaces.Services;
using Store.Application.Utils;
using Store.Domain.Entities;
using Store.Domain.Entities.Interfaces;
using Store.WebApplicationMVC.Models;
using Store.WebApplicationMVC.ViewModel;

namespace Store.WebApplicationMVC.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly IUserOrderService _userOrderService;
        private readonly ICartService _cartService;
        public OrderController(IUserContext userContext, IUserOrderService userOrderService, ICartService cartService)
        {
            _userContext = userContext;
            _userOrderService = userOrderService;
            _cartService = cartService;
        }
        private int PageSize { get; } = 20;
        [Authorize]
        public async Task<IActionResult> Index(int page = 1, string[]? orderStatuses = null)
        {
            if (!_userContext.IsAuthenticated || _userContext.UserId == null)
            {
                return View("Error");
            }

            var orders = orderStatuses == null ?
                await _userOrderService.GetAllOrdersForUserAsync(_userContext.UserId.Value, 1, PageSize) :
                await _userOrderService.GetAllOrdersOfStatusForUserAsync(_userContext.UserId.Value, 1, PageSize, orderStatuses);

            return View(new OrdersListViewModel()
            {
                Orders = orders,
                PagingInfo = new PagingInfo()
                {
                    PageSize = PageSize,
                    HasNextPage = orders.HasNextPage,
                    HasPreviousPage = orders.HasPreviousPage,
                    Page = page,
                    TotalCount = orders.TotalCount,
                    TotalPages = orders.TotalPages,
                },
                OrderStatuses = EnumValuesGetter.GetAllValues<OrderStatus>().Select(s => new OrderStatusViewModel()
                {
                    Status = s,
                    IsChecked = orderStatuses?.Contains(s) ?? false
                }).ToArray(),
            });
        }
        [Authorize]
        public async Task<IActionResult> CreateOrderFromCart()
        {
            if (!_userContext.IsAuthenticated || _userContext.UserId == null)
            {
                return View("Error");
            }

            await _userOrderService.CreateOrderAsync(_userContext.UserId.Value, _cartService.Lines.ToArray());
            _cartService.Clear();
            return RedirectToAction("Index", new { page = 1 });
        }
        [Authorize]
        public async Task<IActionResult> Details(Guid orderId)
        {
            return View(new OrderDetailViewModel()
            {
                Order = await _userOrderService.GetOrderAsync(orderId),
            });
        }
        [Authorize]
        [HttpPost]
        public IActionResult FilterOrdersByStatuses([FromForm] OrderStatusViewModel[] statusViewModels)
        {
            var statuses = statusViewModels.Where(svm => svm.IsChecked).
                Select(svm => svm.Status).
                ToArray();

            return RedirectToAction("Index", new { orderStatuses = statuses.Count() > 0 ? statuses : null});
        }
    }
}
