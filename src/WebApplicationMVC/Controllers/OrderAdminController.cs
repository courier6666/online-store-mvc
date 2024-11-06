using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Interfaces.Services;
using Store.Application.Queries;
using Store.Application.Services;
using Store.Application.Utils;
using Store.Domain.Entities;
using Store.Domain.Entities.Interfaces;
using Store.Domain.Entities.Model;
using Store.WebApplicationMVC.Models;
using Store.WebApplicationMVC.ViewModel;

namespace Store.WebApplicationMVC.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class OrderAdminController : Controller
    {
        private readonly IAdminOrderService _adminOrderService;
        private readonly IUserService _userService;
        private readonly IUserContext _userContext;
        public OrderAdminController(IAdminOrderService adminOrderService, IUserContext userContext, IUserService userService)
        {
            _adminOrderService = adminOrderService;
            _userContext = userContext;
            _userService = userService;
        }
        int PageSize { get; } = 20;
        public async Task<IActionResult> Index(int page = 1, string[]? orderStatuses = null, Guid? userId = null, Guid? orderId = null)
        {
            var orders = await _adminOrderService.GetPagedOrdersAsync(new OrdersAdminPageQuery()
            {
                PageSize = PageSize,
                OrderId = orderId,
                UserId = userId,
                OrderStatuses = orderStatuses,
                Page = page,
            });

            return View(new AdminOrdersViewModel()
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
                }).ToArray()
            });
        }
        public async Task<IActionResult> Details(Guid orderId)
        {
            var foundOrder = await _adminOrderService.GetOrderAsync(orderId);
            return View(new AdminOrderDetailViewModel()
            {
                Order = foundOrder,
            });
        }
        [HttpPost]
        public IActionResult FilterOrders([FromForm] AdminOrdersViewModel adminOrdersViewModel)
        {
            var statuses = adminOrdersViewModel.OrderStatuses.Where(svm => svm.IsChecked).
                Select(svm => svm.Status).
                ToArray();

            return RedirectToAction("Index", new { orderStatuses = statuses.Count() > 0 ? statuses : null,
                userId = adminOrdersViewModel.UserId,
                orderId = adminOrdersViewModel.OrderId,
            });
        }
        [HttpPost]
        public async Task<IActionResult> ReceivePaymentForOrder(Guid orderId)
        {
            try
            {
                await _adminOrderService.ReceivePaymentForOrderAsync(orderId, _userContext.UserId.Value);
            }
            catch(InvalidOperationException ex)
            {
                this.ModelState.AddModelError("", ex.Message);
            }
            var foundOrder = await _adminOrderService.GetOrderAsync(orderId);
            return View("Details", new AdminOrderDetailViewModel()
            {
                Order = foundOrder,
            });
        }
        [HttpPost]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            try
            {
                await _adminOrderService.CancelOrderAsync(orderId, _userContext.UserId.Value);
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError("", ex.Message);

            }
            var foundOrder = await _adminOrderService.GetOrderAsync(orderId);
            return View("Details", new AdminOrderDetailViewModel()
            {
                Order = foundOrder,
            });
        }
        [HttpPost]
        public async Task<IActionResult> SendOrder(Guid orderId)
        {
            try
            {
                await _adminOrderService.SendOrderAsync(orderId, _userContext.UserId.Value);
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError("", ex.Message);
            }

            var foundOrder = await _adminOrderService.GetOrderAsync(orderId);
            return View("Details", new AdminOrderDetailViewModel()
            {
                Order = foundOrder,
            });
        }
        [HttpPost]
        public async Task<IActionResult> ReceiveOrder(Guid orderId)
        {
            try
            {
                await _adminOrderService.ReceiveOrderAsync(orderId, _userContext.UserId.Value);
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError("", ex.Message);
            }

            var foundOrder = await _adminOrderService.GetOrderAsync(orderId);
            return View("Details", new AdminOrderDetailViewModel()
            {
                Order = foundOrder,
            });
        }
        [HttpPost]
        public async Task<IActionResult> CompleteOrder(Guid orderId)
        {
            try
            {
                await _adminOrderService.CompleteOrderAsync(orderId, _userContext.UserId.Value);
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError("", ex.Message);
            }
            var foundOrder = await _adminOrderService.GetOrderAsync(orderId);
            return View("Details", new AdminOrderDetailViewModel()
            {
                Order = foundOrder,
            });
        }
        [HttpPost]
        public async Task<IActionResult> ChangeOrderStatusManually(Guid orderId, string status)
        {
            try
            {
                await _adminOrderService.ChangeOrderStatusAsync(orderId, status, _userContext.UserId.Value);
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError("", ex.Message);
            }

            var foundOrder = await _adminOrderService.GetOrderAsync(orderId);
            return View("Details", new AdminOrderDetailViewModel()
            {
                Order = foundOrder,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid orderId)
        {
            try
            {
                await _adminOrderService.DeleteOrderAsync(orderId, _userContext.UserId.Value);
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError("", ex.Message);
            }

            return RedirectToAction("Index");
        }
    }
}
