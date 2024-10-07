using Store.Application.DataTransferObjects;
using Store.Application.Interfaces.Mapper;
using Store.Application.Interfaces.Services;
using Store.Domain.Entities;
using Store.Domain.Entities.Interfaces;
using Store.Domain.Entities.Model;
using Store.Domain.PagedLists;
using Store.Domain.StringToEnumConverter;
using System.Linq.Expressions;

namespace Store.Application.Services
{
    /// <summary>
    /// Provides services for managing user orders including creating, retrieving,
    /// adding products to orders, canceling, and paying for orders.
    /// </summary>
    public sealed class UserOrderService : IUserOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomMapper _customMapper;
        public UserOrderService(IUnitOfWork unitOfWork, ICustomMapper customMapper)
        {
            _unitOfWork = unitOfWork;
            _customMapper = customMapper;
        }
        /// <summary>
        /// Retrieves all orders for a specific user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A collection of order DTOs.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the user is not found.</exception>
        public async Task<IEnumerable<OrderDto>> GetAllUserOrders(Guid userId)
        {
            var foundUser = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (foundUser == null)
                throw new InvalidOperationException($"User by id '{userId}' not found.");

            IEnumerable<Order> foundOrders =
                await _unitOfWork.OrderRepository.GetAllOrdersOfUserIncludingProductDetailsAndProductsAsync(userId);

            return _customMapper.MapEnumerable<Order, OrderDto>(foundOrders);
        }
        /// <summary>
        /// Adds a product to an existing order.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="orderId">The order ID.</param>
        /// <param name="productDetailsDto">The product details DTO.</param>
        /// <exception cref="InvalidOperationException">
        /// // Thrown when the order, user, or cash deposit is not found, or the order status does not allow payment.
        /// </exception>
        public async Task AddProductToExistingOrderAsync(Guid userId, Guid orderId, ProductDetailsDto productDetailsDto)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                IUser foundUser = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (foundUser == null)
                    throw new InvalidOperationException($"User by id '{userId}' not found!");

                Order foundOrder = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
                if (foundOrder == null)
                    throw new InvalidOperationException($"Order by id '{orderId}' not found!");

                Product foundProduct = await _unitOfWork.ProductRepository.GetByIdAsync(productDetailsDto.Product.Id.Value);
                if (foundProduct == null)
                    throw new InvalidOperationException($"Product by id '{productDetailsDto.Product.Id.Value}' not found.");

                OrderProductDetail orderProductDetail = _customMapper.Map<ProductDetailsDto, OrderProductDetail>(productDetailsDto);

                _unitOfWork.OrderRepository.AddOrderDetailsToOrder(foundOrder.Id, orderProductDetail);

                _unitOfWork.OrderEntryRepository.Add(new Entry()
                {
                    OrderId = foundOrder.Id,
                    Message = $"Added new product - '{orderProductDetail.Product.Name}' x {orderProductDetail.ItemsCount}, to order '{foundOrder.Id}'."
                });
                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException($"Failed to add product to existing order!", innerException: e);
            }
        }
        /// <summary>
        /// Cancels an order.
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        /// <param name="userId">The user ID.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the order or user is not found, the order does not belong to the user, 
        /// the order has already been canceled, or the order status does not allow cancellation.
        /// </exception>
        public async Task CancelOrderAsync(Guid orderId, Guid userId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                Order foundOrder = await _unitOfWork.OrderRepository.GetOrderWithPaymentDetailsAsync(orderId);

                if (foundOrder == null)
                    throw new InvalidOperationException($"Order by id '{orderId}' not found!");

                IUser foundUser = await _unitOfWork.UserRepository.GetByIdAsync(userId);

                if (foundUser == null)
                    throw new InvalidOperationException($"User by id '{userId}' not found!");

                if (foundOrder.OrderAuthorId != userId)
                    throw new InvalidOperationException($"Provided order by id '{orderId}' does not belong to user with id '{userId}'.");

                if (foundOrder.Status == OrderStatus.CancelledByUser || foundOrder.Status == OrderStatus.CancelledByAdmin)
                    throw new InvalidOperationException($"Order by id '{foundOrder.Id}' has been already cancelled!");

                if (foundOrder.Status >= OrderStatus.Sent)
                    throw new InvalidOperationException(
                        $"Order can be cancelled by user only before being sent! Order with id ({foundOrder.Id}) with status '{foundOrder.Status}'");

                if (foundOrder.IsOrderPayed)
                {
                    foundOrder.IsOrderPayed = false;
                    var cashDeposit = await _unitOfWork.
                            CashDepositRepository.
                            GetByIdAsync(foundOrder.PaymentDetails.CashDepositId);

                    cashDeposit.DepositAmount(foundOrder.PaymentDetails.AmountPayed);
                    _unitOfWork.PaymentDetailsRepository.Delete(foundOrder.PaymentDetails);
                    foundOrder.PaymentDetails = null;
                }

                foundOrder.Status = OrderStatus.CancelledByUser;

                _unitOfWork.OrderRepository.Update(foundOrder);


                _unitOfWork.OrderEntryRepository.Add(new Entry()
                {
                    OrderId = foundOrder.Id,
                    Message = $"Order '{foundOrder.Id}' has been cancelled by user"
                });

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException($"Failed to cancel order for user!", innerException: ex);
            }
        }
        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="productDetailsDto">The product details DTO.</param>
        /// <returns>The ID of the newly created order.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the user or product is not found.</exception>
        public async Task<Guid> CreateOrderAsync(Guid userId, ProductDetailsDto productDetailsDto)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                IUser foundUser = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (foundUser == null)
                    throw new InvalidOperationException($"User by id '{userId}' not found!");

                Product foundProduct = await _unitOfWork.ProductRepository.GetByIdAsync(productDetailsDto.Product.Id.Value);
                if (foundProduct == null)
                    throw new InvalidOperationException($"Product by id '{productDetailsDto.Product.Id.Value}' not found!");

                Order createdOrder = new Order()
                {
                    CreatedDate = DateTime.Now,
                    Status = OrderStatus.New,
                    OrderAuthorId = userId
                };

                createdOrder.OrderAuthorId = userId;
                _unitOfWork.OrderRepository.Add(createdOrder);

                OrderProductDetail productDetail = _customMapper.Map<ProductDetailsDto, OrderProductDetail>(productDetailsDto);
                _unitOfWork.OrderRepository.AddOrderDetailsToOrder(createdOrder.Id, productDetail);

                _unitOfWork.OrderEntryRepository.Add(new Entry()
                {
                    OrderId = createdOrder.Id,
                    Message = $"Order '{createdOrder.Id}' has been created by user. Total price: {(productDetailsDto.Product.Price * productDetailsDto.ItemsCount).ToString("c")}."
                });

                await _unitOfWork.CommitAsync();
                return createdOrder.Id;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException("Failed to create order!", innerException: e);
            }
        }
        /// <summary>
        /// Pays for an order.
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        /// <param name="userId">The user ID.</param>
        /// <param name="cashDepositId">The cash deposit ID.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the order, user, or cash deposit is not found, or the order status does not allow payment.
        /// </exception>
        public async Task PayForOrderAsync(Guid orderId, Guid userId, Guid cashDepositId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                Order foundOrder = await _unitOfWork.OrderRepository.GetOrderByIdIncludingOrderAndProductDetailsAsync(orderId);

                if (foundOrder == null)
                    throw new InvalidOperationException($"Order by id '{orderId}' not found.");

                if (foundOrder.Status == OrderStatus.PaymentTransferred)
                    throw new InvalidOperationException($"Order with id ({foundOrder.Id}) has been already payed by user.");

                if (foundOrder.Status != OrderStatus.New)
                    throw new InvalidOperationException($"Order with id ({foundOrder.Id}) must have status '{OrderStatus.New}', in order to be payed.");


                IUser foundUser = await _unitOfWork.UserRepository.GetByIdAsync(userId);

                if (foundUser == null)
                    throw new InvalidOperationException($"User by id '{userId}' not found.");

                CashDeposit foundCashDeposit = await _unitOfWork.CashDepositRepository.GetByIdAsync(cashDepositId);

                if (foundCashDeposit == null)
                    throw new InvalidOperationException($"Cash deposit by id '{cashDepositId}' not found.");

                if (foundOrder.OrderAuthorId != foundUser.Id)
                    throw new InvalidOperationException($"Order with id '{orderId}' has a different author with a different id.");

                if (foundCashDeposit.UserId != foundUser.Id)
                    throw new InvalidOperationException($"Cash Deposit with id '{orderId}' has a different owner with a different id.");

                foundCashDeposit.WithdrawAmount(foundOrder.TotalPrice);

                PaymentDetails paymentDetails = new PaymentDetails()
                {
                    AmountPayed = foundOrder.TotalPrice,
                    CashDepositId = foundCashDeposit.Id,
                    OrderId = foundOrder.Id
                };

                foundOrder.Status = OrderStatus.PaymentTransferred;
                foundOrder.IsOrderPayed = true;

                _unitOfWork.OrderRepository.Update(foundOrder);
                _unitOfWork.CashDepositRepository.Update(foundCashDeposit);
                _unitOfWork.PaymentDetailsRepository.Add(paymentDetails);

                _unitOfWork.OrderEntryRepository.Add(new Entry()
                {
                    OrderId = foundOrder.Id,
                    Message = $"Order '{foundOrder.Id}' has been payed by user. Total amount payed - {paymentDetails.AmountPayed}."
                });

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException("Failed to pay for order.", innerException: ex);
            }
        }

        public async Task<Guid> CreateOrderAsync(Guid userId, ProductDetailsDto[] productDetailsDto)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                IUser foundUser = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (foundUser == null)
                    throw new InvalidOperationException($"User by id '{userId}' not found!");

                Order createdOrder = new Order()
                {
                    CreatedDate = DateTime.Now,
                    Status = OrderStatus.New,
                    OrderAuthorId = userId
                };

                createdOrder.OrderAuthorId = userId;
                _unitOfWork.OrderRepository.Add(createdOrder);



                foreach (var product in productDetailsDto)
                {
                    Product foundProduct = await _unitOfWork.ProductRepository.GetByIdAsync(product.Product.Id.Value);
                    if (foundProduct == null)
                        throw new InvalidOperationException($"Product by id '{product.Product.Id.Value}' not found!");

                    OrderProductDetail productDetail = _customMapper.Map<ProductDetailsDto, OrderProductDetail>(product);
                    productDetail.Product = null;

                    _unitOfWork.OrderRepository.AddOrderDetailsToOrder(createdOrder.Id, productDetail);
                    _unitOfWork.OrderEntryRepository.Add(new Entry()
                    {
                        OrderId = createdOrder.Id,
                        Message = $"Product '{foundProduct.Name}' has been added to order '{createdOrder.Id}'."
                    });
                }

                _unitOfWork.OrderEntryRepository.Add(new Entry()
                {
                    OrderId = createdOrder.Id,
                    Message = $"Order '{createdOrder.Id}' has been created by user. Total price:"
                });

                await _unitOfWork.CommitAsync();
                return createdOrder.Id;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException("Failed to create order!", innerException: e);
            }
        }

        public async Task<PagedList<OrderDto>> GetAllOrdersForUserAsync(Guid userId, int page, int pageSize)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var pagedOrders = await _unitOfWork.OrderRepository.GetPagedListFilterAndOrderAsync(page, pageSize, o => o.OrderAuthorId == userId, o => o.CreatedDate);
                await _unitOfWork.CommitAsync();
                return _customMapper.MapPagedList<Order, OrderDto>(pagedOrders);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }

        public async Task<PagedList<OrderDto>> GetAllOrdersOfStatusForUserAsync(Guid userId, int page, int pageSize, string[] statuses)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                OrderStatus[] statusesParsed = statuses.Select(s => StringToEnumConverter.ConvertStringToEnumValue<OrderStatus>(s)).
                    Where(status => status != null).
                    Select(status => status.Value).
                    ToArray();

                var pagedOrders = await _unitOfWork.OrderRepository.GetPagedListFilterAndOrderAsync(page,
                    pageSize,
                    o => o.OrderAuthorId == userId && (statusesParsed.Length == 0 || statusesParsed.Contains(o.Status)),
                    o => o.CreatedDate);

                await _unitOfWork.CommitAsync();
                return _customMapper.MapPagedList<Order, OrderDto>(pagedOrders);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }

        public async Task<OrderDto> GetOrderAsync(Guid orderId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var order = await _unitOfWork.OrderRepository.GetOrderByIdIncludingOrderAndProductDetailsAsync(orderId);

                await _unitOfWork.CommitAsync();
                return _customMapper.Map<Order, OrderDto>(order);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }
    }
}
