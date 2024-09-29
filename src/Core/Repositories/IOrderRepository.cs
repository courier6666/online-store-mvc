using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Store.Domain.Entities;
using Store.Domain.PagedLists;

namespace Store.Domain.Repositories
{
    public interface IOrderRepository : IPagedListRepository<Order, Guid>
    {
        /// <summary>
        /// Used for getting all products with quantity included in order.
        /// </summary>
        /// <param name="orderId">Id of order <typeparamref name="Order"/></param>
        /// <returns>Collection of all products details.</returns>
        Task<IEnumerable<OrderProductDetail>> GetAllOrderDetailsForOrderAsync(Guid orderId);
        /// <summary>
        /// Gets order by id including all products in order.
        /// </summary>
        /// <param name="orderId">Id of order <typeparamref name="Order"/></param>
        /// <returns><typeparamref name="Order"/></returns>
        Task<Order> GetOrderByIdIncludingOrderAndProductDetailsAsync(Guid orderId);
        /// <summary>
        /// Gets order by id including order author.
        /// </summary>
        /// <param name="orderId">Id of order <typeparamref name="Order"/></param>
        /// <returns><typeparamref name="Order"/></returns>
        Task<Order> GetOrderByIdIncludingAuthorAsync(Guid orderId);
        /// <summary>
        /// Gets order by id including all products details and products.
        /// </summary>
        /// <returns>Collections of orders</returns>
        Task<IEnumerable<Order>> GetAllOrdersIncludingOrderAndProductDetailsAsync();
        /// <summary>
        /// Gets all orders of certain status <typeparamref name="OrderStatus"/>
        /// </summary>
        /// <param name="status">Status of orders</param>
        /// <returns>Collections of orders of status <typeparamref name="OrderStatus"/></returns>
        Task<IEnumerable<Order>> GetAllOrdersByStatusAsync(OrderStatus status);
        /// <summary>
        /// Gets all orders including order history entries.
        /// </summary>
        /// <returns>Collections of orders with entries <typeparamref name="Entry"/></returns>
        Task<IEnumerable<Order>> GetAllOrdersWithEntriesAsync();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId">Id of ord</param>
        /// <returns></returns>
        Task<Order> GetOrderByIdWithEntriesAsync(Guid orderId);
        /// <summary>
        /// Add product to order, including quantity.
        /// </summary>
        /// <param name="orderId">Id of order</param>
        /// <param name="orderProductDetail">Product with quantity to add to order</param>
        void AddOrderDetailsToOrder(Guid orderId, OrderProductDetail orderProductDetail);
        /// <summary>
        /// Change product's info in order.
        /// </summary>
        /// <param name="orderProductDetail">Product details, include product an quantity.</param>
        void UpdateOrderDetails(OrderProductDetail orderProductDetail);
        /// <summary>
        /// Gets order including payment details.
        /// </summary>
        /// <param name="orderId">Id of order</param>
        /// <returns>Order with payment details.</returns>
        Task<Order> GetOrderWithPaymentDetailsAsync(Guid orderId);
        /// <summary>
        /// Gets all orders of user including its products.
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <returns>Collections of orders of user including products.</returns>
        Task<IEnumerable<Order>> GetAllOrdersOfUserIncludingProductDetailsAndProductsAsync(Guid userId);
        Task<IEnumerable<Order>> GetByFilterAsync(Expression<Func<Order, bool>> filter);
    }
}
