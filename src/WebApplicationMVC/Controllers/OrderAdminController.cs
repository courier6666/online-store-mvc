using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Interfaces.Services;
using Store.Application.Queries;
using Store.Application.Utils;
using Store.Domain.Entities;
using Store.Domain.Entities.Model;
using Store.WebApplicationMVC.Models;
using Store.WebApplicationMVC.ViewModel;

namespace Store.WebApplicationMVC.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class OrderAdminController : Controller
    {
        private readonly IAdminOrderService _adminOrderService;
        public OrderAdminController(IAdminOrderService adminOrderService)
        {
            _adminOrderService = adminOrderService;
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
    }
}
