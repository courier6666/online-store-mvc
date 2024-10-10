using Store.Application.DataTransferObjects;
using Store.Application.Interfaces.Mapper;
using Store.Application.Interfaces.Services;
using Store.Application.Queries;
using Store.Application.Utils;
using Store.Domain.Entities;
using Store.Domain.Entities.Interfaces;
using Store.Domain.Entities.Model;
using Store.Domain.PagedLists;
using Store.Domain.StringToEnumConverter;
using System.Linq.Expressions;

namespace Store.Application.Services
{
    /// <summary>
    /// The AdminOrderService class provides a set of functionalities for managing orders
    /// and products within an admin context. It includes use-cases for viewing products,
    /// creating orders, adding products to orders, viewing orders, paying for orders,
    /// receiving payments, sending orders, receiving orders, completing orders, 
    /// canceling orders, changing order status manually, refunding orders, 
    /// getting order history, creating cash deposits, and viewing cash deposits.
    /// </summary>
    public sealed class AdminOrderService : IAdminOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomMapper _customMapper;

        public AdminOrderService(IUnitOfWork unitOfWork, ICustomMapper customMapper)
        {
            _unitOfWork = unitOfWork;
            _customMapper = customMapper;
        }
        /// <summary>
        /// Adds product with quantity to existing order.
        /// </summary>
        /// <param name="userId">Owner of order.</param>
        /// <param name="orderId">Order id</param>
        /// <param name="productDetailsDto">Product info: product id and quantity</param>
        /// <exception cref="InvalidOperationException">Thrown when user, order or product is not found.</exception>
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
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }
        /// <summary>
        /// Order gets cancelled by admin. Status of order changed to <typeparamref name="Cancelled"/>
        /// </summary>
        /// <param name="orderId">Order</param>
        /// <param name="adminId">Admin</param>
        /// <exception cref="InvalidOperationException">Thrown when order, admin is not found; found user by id is not an admin; order has been already cancelled</exception>
        public async Task CancelOrderAsync(Guid orderId, Guid adminId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                Order foundOrder = await _unitOfWork.OrderRepository.GetOrderWithPaymentDetailsAsync(orderId);

                if (foundOrder == null)
                    throw new InvalidOperationException($"Order by id '{orderId}' not found!");

                IUser foundAdmin = await _unitOfWork.UserRepository.GetByIdAsync(adminId);

                if (foundAdmin == null)
                    throw new InvalidOperationException($"Admin by id '{adminId}' not found!");

                if (!foundAdmin.Roles.Contains(Roles.Admin))
                    throw new InvalidOperationException($"Found user by id '{adminId}' is not an administrator.");

                if (foundOrder.Status == OrderStatus.CancelledByUser || foundOrder.Status == OrderStatus.CancelledByAdmin)
                    throw new InvalidOperationException($"Order by id '{foundOrder.Id}' has been already cancelled!");

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

                foundOrder.Status = OrderStatus.CancelledByAdmin;

                _unitOfWork.OrderEntryRepository.Add(new Entry()
                {
                    OrderId = foundOrder.Id,
                    Message = $"Order '{foundOrder.Id}' has been cancelled by admin."
                });

                _unitOfWork.OrderRepository.Update(foundOrder);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }
        /// <summary>
        /// Changed status of order manually. Ensures that status is changed by admin with permissions.
        /// Status is changed to argument newStatus.
        /// </summary>
        /// <param name="orderId">Order</param>
        /// <param name="newStatus">Status to change to</param>
        /// <param name="adminId">Admin</param>
        /// <exception cref="InvalidOperationException">Throws when provided status is not found; order or admin is not found;found user by id is not an admin</exception>
        public async Task ChangeOrderStatusAsync(Guid orderId, string newStatus, Guid adminId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                OrderStatus status = StringToEnumConverter.ConvertStringToEnumValue<OrderStatus>(newStatus) ??
                                     throw new ArgumentException($"There is no such status '{newStatus}'.", nameof(newStatus));

                Order foundOrder = await _unitOfWork.OrderRepository.GetOrderWithPaymentDetailsAsync(orderId);

                if (foundOrder == null)
                    throw new InvalidOperationException($"Order by id '{orderId}' not found!");

                IUser foundAdmin = await _unitOfWork.UserRepository.GetByIdAsync(adminId);

                if (foundAdmin == null)
                    throw new InvalidOperationException($"Admin by id '{adminId}' not found!");

                if (!foundAdmin.Roles.Contains(Roles.Admin))
                    throw new InvalidOperationException($"Found user by id '{adminId}' is not an administrator.");

                foundOrder.Status = status;

                _unitOfWork.OrderRepository.Update(foundOrder);

                _unitOfWork.OrderEntryRepository.Add(new Entry()
                {
                    OrderId = foundOrder.Id,
                    Message = $"Order '{foundOrder.Id}' status has been changed to '{foundOrder.Status}'."
                });

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }
        /// <summary>
        /// Order gets completed. Status of order changed to <typeparamref name="Completed"/>
        /// </summary>
        /// <param name="orderId">Order</param>
        /// <param name="adminId">Admin</param>
        /// <exception cref="InvalidOperationException">Thrown when admin or order is not found; found user by id is not an admin; order status is not 'Received'</exception>
        public async Task CompleteOrderAsync(Guid orderId, Guid adminId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                IUser foundAdmin = await _unitOfWork.UserRepository.GetByIdAsync(adminId);

                if (foundAdmin == null)
                    throw new InvalidOperationException($"Admin by id '{adminId}' not found!");

                if (!foundAdmin.Roles.Contains(Roles.Admin))
                    throw new InvalidOperationException($"Found user by id '{adminId}' is not an administrator.");

                Order foundOrder = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);

                if (foundOrder == null)
                    throw new InvalidOperationException($"Order by id '{orderId}' not found!");

                if (foundOrder.Status != OrderStatus.Received)
                    throw new InvalidOperationException($"Order by id '{foundOrder.Id}' must be first received by user in order for order to be completed by administrator.");

                foundOrder.Status = OrderStatus.Completed;

                _unitOfWork.OrderRepository.Update(foundOrder);

                _unitOfWork.OrderEntryRepository.Add(new Entry()
                {
                    OrderId = foundOrder.Id,
                    Message = $"Order '{foundOrder.Id}' has been completed."
                });

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }
        /// <summary>
        /// Creates order for user, adding product to order
        /// </summary>
        /// <param name="userId">User</param>
        /// <param name="productDetailsDto">Product added to newly created order.</param>
        /// <returns>Id of newly created order</returns>
        /// <exception cref="InvalidOperationException">Thrown when user or product is not found</exception>
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
                    Message = $"Order '{createdOrder.Id}' has been created by user."
                });

                await _unitOfWork.CommitAsync();
                return createdOrder.Id;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }

        public async Task<Guid> CreateOrderAsync(Guid adminId, ProductDetailsDto[] productDetailsDto)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                IUser foundUser = await _unitOfWork.UserRepository.GetByIdAsync(adminId);
                if (foundUser == null)
                    throw new InvalidOperationException($"User by id '{adminId}' not found!");

                Order createdOrder = new Order()
                {
                    CreatedDate = DateTime.Now,
                    Status = OrderStatus.New,
                    OrderAuthorId = adminId
                };

                createdOrder.OrderAuthorId = adminId;
                _unitOfWork.OrderRepository.Add(createdOrder);

                _unitOfWork.OrderEntryRepository.Add(new Entry()
                {
                    OrderId = createdOrder.Id,
                    Message = $"Order '{createdOrder.Id}' has been created by user."
                });

                foreach (var product in productDetailsDto)
                {
                    Product foundProduct = await _unitOfWork.ProductRepository.GetByIdAsync(product.Product.Id.Value);
                    if (foundProduct == null)
                        throw new InvalidOperationException($"Product by id '{product.Product.Id.Value}' not found!");

                    OrderProductDetail productDetail = _customMapper.Map<ProductDetailsDto, OrderProductDetail>(product);
                    _unitOfWork.OrderRepository.AddOrderDetailsToOrder(createdOrder.Id, productDetail);
                }

                await _unitOfWork.CommitAsync();
                return createdOrder.Id;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// Creates <typeparamref name="HashSet"/> with all order statuses.
        /// </summary>
        /// <returns><typeparamref name="HashSet"/> with strings that represent all order statuses.</returns>
        public HashSet<string> GetAllOrderStatuses()
        {
            return new HashSet<string>(EnumValuesGetter.GetAllValues<OrderStatus>());
        }
        /// <summary>
        /// Finds and returns all orders stored in database.
        /// </summary>
        /// <returns><typeparamref name="IEnumerable"/> with <typeparamref name="OrderDto"/></returns>
        public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
        {

            IEnumerable<Order> foundOrders =
                await _unitOfWork.OrderRepository.GetAllOrdersIncludingOrderAndProductDetailsAsync();

            return _customMapper.MapEnumerable<Order, OrderDto>(foundOrders);
        }

        public async Task<PagedList<OrderDto>> GetPagedOrdersAsync(OrdersAdminPageQuery adminOrdersPageQuery)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                OrderStatus[] statusesParsed = adminOrdersPageQuery.OrderStatuses?.Select(s => StringToEnumConverter.ConvertStringToEnumValue<OrderStatus>(s)).
                    Where(status => status != null).
                    Select(status => status.Value).
                    ToArray() ?? new OrderStatus[0];

                Expression<Func<Order, bool>> exp = o =>
                (adminOrdersPageQuery.OrderId == null || o.Id == adminOrdersPageQuery.OrderId) &&
                (adminOrdersPageQuery.UserId == null || o.OrderAuthorId == adminOrdersPageQuery.OrderId) &&
                (statusesParsed.Length == 0 || statusesParsed.Contains(o.Status));

                var pagedOrders = await _unitOfWork.OrderRepository.GetPagedListFilterAndOrderDescAsync(adminOrdersPageQuery.Page,
                    adminOrdersPageQuery.PageSize,
                    exp,
                    o => o.CreatedDate);

                await _unitOfWork.CommitAsync();

                return _customMapper.MapPagedList<Order, OrderDto>(pagedOrders);
            }
            catch(Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// Pays for order by user from provided cash deposit. Order status gets changed to <typeparamref name="PaymentTransferred"/>.
        /// Order status must be <typeparamref name="New"/> first.
        /// </summary>
        /// <param name="orderId">Order</param>
        /// <param name="userId">User</param>
        /// <param name="cashDepositId">Cash Deposit</param>
        /// <exception cref="InvalidOperationException">Thrown when order or user is not found; order status is not 'New'</exception>
        public async Task PayForOrderAsync(Guid orderId, Guid userId, Guid cashDepositId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                Order foundOrder = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);

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
                throw ex;
            }
        }
        /// <summary>
        /// Sets order to status <typeparamref name="Received"/> by admin that has permissions.
        /// Order must be sent first - status must be <typeparamref name="Sent"/>
        /// </summary>
        /// <param name="orderId">Order</param>
        /// <param name="adminId">Admin</param>
        /// <exception cref="InvalidOperationException">Thrown when admin or order is not found; found user is not an admin; status of order is not 'Sent'</exception>
        public async Task ReceiveOrderAsync(Guid orderId, Guid adminId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                IUser foundAdmin = await _unitOfWork.UserRepository.GetByIdAsync(adminId);

                if (foundAdmin == null)
                    throw new InvalidOperationException($"Admin by id '{adminId}' not found!");

                if (!foundAdmin.Roles.Contains(Roles.Admin))
                    throw new InvalidOperationException($"Found user by id '{adminId}' is not an administrator.");

                Order foundOrder = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);

                if (foundOrder == null)
                    throw new InvalidOperationException($"Order by id '{orderId}' not found!");

                if (foundOrder.Status != OrderStatus.Sent)
                    throw new InvalidOperationException($"Order by id '{foundOrder.Id}' must be first sent before being received by user.");

                foundOrder.Status = OrderStatus.Received;

                _unitOfWork.OrderRepository.Update(foundOrder);

                _unitOfWork.OrderEntryRepository.Add(new Entry()
                {
                    OrderId = foundOrder.Id,
                    Message = $"Order '{foundOrder.Id}' has been received by user."
                });

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }
        /// <summary>
        /// Sets order to status <typeparamref name="PaymentReceived"/> by admin that has permissions.
        /// Order must be paid by user first - status must be <typeparamref name="PaymentTransferred"/>
        /// </summary>
        /// <param name="orderId">Order</param>
        /// <param name="adminId">Admin</param>
        /// <exception cref="InvalidOperationException">Thrown when admin or order is not found; found user is not an admin; order status is not 'PaymentTransferred' or order is not payed</exception>
        public async Task ReceivePaymentForOrderAsync(Guid orderId, Guid adminId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                IUser foundAdmin = await _unitOfWork.UserRepository.GetByIdAsync(adminId);

                if (foundAdmin == null)
                    throw new InvalidOperationException($"Admin by id '{adminId}' not found!");

                if (!foundAdmin.Roles.Contains(Roles.Admin))
                    throw new InvalidOperationException($"Found user by id '{adminId}' is not an administrator.");

                Order foundOrder = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);

                if (foundOrder == null)
                    throw new InvalidOperationException($"Order by id '{orderId}' not found!");

                if (foundOrder.Status != OrderStatus.PaymentTransferred || !foundOrder.IsOrderPayed)
                    throw new InvalidOperationException($"Order by id '{foundOrder.Id}' must be first payed by user in order for payment to be received by administrator.");

                foundOrder.Status = OrderStatus.PaymentReceived;

                _unitOfWork.OrderRepository.Update(foundOrder);

                _unitOfWork.OrderEntryRepository.Add(new Entry()
                {
                    OrderId = foundOrder.Id,
                    Message = $"Payment for order '{foundOrder.Id}' has been received."
                });

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }
        /// <summary>
        /// Refunds money back to cash deposit, that was used to pay for order.
        /// Order must be paid by user.
        /// </summary>
        /// <param name="orderId">Order</param>
        /// <param name="adminId">Admin</param>
        /// <exception cref="InvalidOperationException">Thrown when admin or user is not found; found user is not an admin; order is not paid or does not have payment details</exception>
        public async Task RefundOrderAsync(Guid orderId, Guid adminId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                IUser foundAdmin = await _unitOfWork.UserRepository.GetByIdAsync(adminId);

                if (foundAdmin == null)
                    throw new InvalidOperationException($"Admin by id '{adminId}' not found!");

                if (!foundAdmin.Roles.Contains(Roles.Admin))
                    throw new InvalidOperationException($"Found user by id '{adminId}' is not an administrator.");

                Order foundOrder = await _unitOfWork.OrderRepository.GetOrderWithPaymentDetailsAsync(orderId);

                if (foundOrder == null)
                    throw new InvalidOperationException($"Order by id '{orderId}' not found!");

                if (!foundOrder.IsOrderPayed)
                    throw new InvalidOperationException($"Order by id '{orderId}' is not payed!");

                if (foundOrder.PaymentDetails == null)
                    throw new InvalidOperationException($"Order by id '{orderId}' does not have payment details!");

                foundOrder.IsOrderPayed = false;
                foundOrder.PaymentDetails.CashDeposit.DepositAmount(foundOrder.TotalPrice);

                _unitOfWork.CashDepositRepository.Update(foundOrder.PaymentDetails.CashDeposit);
                _unitOfWork.PaymentDetailsRepository.Delete(foundOrder.PaymentDetails);

                foundOrder.PaymentDetails = null;
                _unitOfWork.OrderRepository.Update(foundOrder);

                _unitOfWork.OrderEntryRepository.Add(new Entry()
                {
                    OrderId = foundOrder.Id,
                    Message = $"Order '{foundOrder.Id}' has been refunded."
                });

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }
        /// <summary>
        /// Sets order status to <typeparamref name="Sent"/>
        /// Order must be paid by user and admin must have accepted the payment first.
        /// </summary>
        /// <param name="orderId">Order</param>
        /// <param name="adminId">Admin</param>
        /// <exception cref="InvalidOperationException">Thrown when admin or order is not found; found user is not admin; order status is not 'PaymentReceived' or is not paid</exception>
        public async Task SendOrderAsync(Guid orderId, Guid adminId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                IUser foundAdmin = await _unitOfWork.UserRepository.GetByIdAsync(adminId);

                if (foundAdmin == null)
                    throw new InvalidOperationException($"Admin by id '{adminId}' not found!");

                if (!foundAdmin.Roles.Contains(Roles.Admin))
                    throw new InvalidOperationException($"Found user by id '{adminId}' is not an administrator.");

                Order foundOrder = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);

                if (foundOrder == null)
                    throw new InvalidOperationException($"Order by id '{orderId}' not found!");

                if (foundOrder.Status == OrderStatus.Sent)
                    throw new InvalidOperationException($"Order with id '{foundOrder.Id}' has been already sent.");


                if (foundOrder.Status != OrderStatus.PaymentReceived || !foundOrder.IsOrderPayed)
                    throw new InvalidOperationException($"Order by id '{foundOrder.Id}' must be first be payed by user and payment must be received and checked by administrator.");

                foundOrder.Status = OrderStatus.Sent;

                _unitOfWork.OrderRepository.Update(foundOrder);

                _unitOfWork.OrderEntryRepository.Add(new Entry()
                {
                    OrderId = foundOrder.Id,
                    Message = $"Order '{foundOrder.Id}' has been sent to user."
                });

                await _unitOfWork.CommitAsync();
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
                var entries = await _unitOfWork.OrderEntryRepository.GetAllEntriesOfOrderAsync(order.Id);
                await _unitOfWork.CommitAsync();

                var entriesDto = _customMapper.MapEnumerable<Entry, EntryDto>(entries);
                var orderDto = _customMapper.Map<Order, OrderDto>(order);
                orderDto.Entries = entriesDto.OrderByDescending(e => e.CreatedDate).ToList();
                return orderDto;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }
    }
}
