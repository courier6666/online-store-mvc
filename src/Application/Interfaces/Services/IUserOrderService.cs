using Store.Application.DataTransferObjects;
using Store.Domain.PagedLists;

namespace Store.Application.Interfaces.Services
{
    /// <summary>
    /// Provides services for managing user orders including creating, retrieving,
    /// adding products to orders, canceling, and paying for orders.
    /// </summary>
    public interface IUserOrderService
    {
        Task PayForOrderAsync(Guid orderId, Guid userId, Guid cashDepositId);
        Task<Guid> CreateOrderAsync(Guid userId, ProductDetailsDto productDetailsDto);
        Task<Guid> CreateOrderAsync(Guid userId, ProductDetailsDto[] productDetailsDto);
        Task AddProductToExistingOrderAsync(Guid userId, Guid orderId, ProductDetailsDto productDetailsDto);
        Task CancelOrderAsync(Guid orderId, Guid userId);
        Task<PagedList<OrderDto>> GetAllOrdersForUserAsync(Guid userId, int page, int pageSize);
        Task<PagedList<OrderDto>> GetAllOrdersOfStatusForUserAsync(Guid userId, int page, int pageSize, string status);
        Task<OrderDto> GetOrderAsync(Guid orderId);
    }
}
