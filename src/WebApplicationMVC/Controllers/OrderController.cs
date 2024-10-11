using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Interfaces.Services;
using Store.Application.Services;
using Store.Application.Utils;
using Store.Domain.Entities;
using Store.Domain.Entities.Interfaces;
using Store.WebApplicationMVC.Models;
using Store.WebApplicationMVC.ViewModel;
using System.Net.WebSockets;

namespace Store.WebApplicationMVC.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly IUserOrderService _userOrderService;
        private readonly ICashDepositService _cashDepositService;
        private readonly ICartService _cartService;
        public OrderController(IUserContext userContext, IUserOrderService userOrderService, ICartService cartService, ICashDepositService cashDepositService)
        {
            _userContext = userContext;
            _userOrderService = userOrderService;
            _cartService = cartService;
            _cashDepositService = cashDepositService;
        }
        private int PageSize { get; } = 20;
        [Authorize]
        public async Task<IActionResult> Index(int page = 1, string[]? orderStatuses = null)
        {
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
            await _userOrderService.CreateOrderAsync(_userContext.UserId.Value, _cartService.Lines.ToArray());
            _cartService.Clear();
            return RedirectToAction("Index", new { page = 1 });
        }
        [Authorize]
        public async Task<IActionResult> Details(Guid orderId)
        {
            var order = await _userOrderService.GetOrderAsync(orderId);
            if (order.OrderAuthorId != _userContext.UserId.Value)
                return View("Error");

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
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PayForOrder(Guid orderId)
        {
            var order = await _userOrderService.GetOrderAsync(orderId);
            if (order.OrderAuthorId != _userContext.UserId.Value)
                return View("Error");

            try
            {
                var cashDeposit = (await _cashDepositService.GetAllCashDepositsForUser(_userContext.UserId.Value)).First();
                await _userOrderService.PayForOrderAsync(orderId, _userContext.UserId.Value, cashDeposit.Id);
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError("", ex.Message);

            }
            var foundOrder = await _userOrderService.GetOrderAsync(orderId);
            return View("Details", new OrderDetailViewModel()
            {
                Order = foundOrder,
            });
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            var order = await _userOrderService.GetOrderAsync(orderId);
            if (order.OrderAuthorId != _userContext.UserId.Value)
                return View("Error");
            try
            {
                await _userOrderService.CancelOrderAsync(orderId, _userContext.UserId.Value);
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError("", ex.Message);

            }
            var foundOrder = await _userOrderService.GetOrderAsync(orderId);
            return View("Details", new OrderDetailViewModel()
            {
                Order = foundOrder,
            });
        }
    }
}
