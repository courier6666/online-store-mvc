using Store.Application.DataTransferObjects;
using Store.Application.Queries;
using Store.Domain.PagedLists;

namespace Store.Application.Interfaces.Services
{
    /// <summary>
    /// The AdminOrderService interface provides a set of functionalities for managing orders
    /// and products within an admin context. It includes use-cases for viewing products,
    /// creating orders, adding products to orders, viewing orders, paying for orders,
    /// receiving payments, sending orders, receiving orders, completing orders, 
    /// canceling orders, changing order status manually, refunding orders, 
    /// getting order history, creating cash deposits, and viewing cash deposits.
    /// </summary>
    public interface IAdminOrderService
    {
        HashSet<string> GetAllOrderStatuses();
        Task<Guid> CreateOrderAsync(Guid adminId, ProductDetailsDto productDetailsDto);
        Task<Guid> CreateOrderAsync(Guid adminId, ProductDetailsDto[] productDetailsDto);
        Task<IEnumerable<OrderDto>> GetOrdersAsync();
        Task AddProductToExistingOrderAsync(Guid userId, Guid orderId, ProductDetailsDto productDetailsDto);
        Task PayForOrderAsync(Guid orderId, Guid adminId, Guid cashDepositId);
        Task CancelOrderAsync(Guid orderId, Guid adminId);
        Task ChangeOrderStatusAsync(Guid orderId, string newStatus, Guid adminId);
        Task RefundOrderAsync(Guid orderId, Guid adminId);
        Task ReceivePaymentForOrderAsync(Guid orderId, Guid adminId);
        Task SendOrderAsync(Guid orderId, Guid adminId);
        Task ReceiveOrderAsync(Guid orderId, Guid adminId);
        Task CompleteOrderAsync(Guid orderId, Guid adminId);
        Task<PagedList<OrderDto>> GetPagedOrdersAsync(OrdersAdminPageQuery productsPageQuery);
        Task<OrderDto> GetOrderAsync(Guid orderId);

    }
}
