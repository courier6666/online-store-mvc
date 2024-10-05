using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.PagedLists;
using Store.Domain.Repositories;
using Store.Persistence.Main.DatabaseContexts;
using Store.Persistence.Main.PagedListCreatorNm;
using System.Linq.Expressions;

namespace Store.Persistence.Main.Repositories
{
    public class EfOrderRepository : IOrderRepository
    {
        private ApplicationDbContext _context;
        public EfOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Order entity)
        {
            _context.Orders.Add(entity);
        }

        public void Delete(Order entity)
        {
            _context.Orders.Remove(entity);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<IEnumerable<OrderProductDetail>> GetAllOrderDetailsForOrderAsync(Guid orderId)
        {
            return await _context.OrderProductDetails.
                Where(opd => opd.OrderId.Equals(orderId)).
                ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersIncludingOrderAndProductDetailsAsync()
        {
            return await _context.Orders.
                Include(o => o.ProductDetails).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersWithEntriesAsync()
        {
            return await _context.Orders.Include(o => o.Entries).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersByStatusAsync(OrderStatus status)
        {
            return await _context.Orders.
                Where(o => o.Status.Equals(status)).
                ToListAsync();
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id.Equals(id));
        }

        public async Task<Order> GetOrderByIdIncludingAuthorAsync(Guid orderId)
        {
            return await _context.Orders.
                Include(o => o.OrderAuthor).
                FirstOrDefaultAsync(o => o.Id.Equals(orderId));
        }

        public async Task<Order> GetOrderByIdIncludingOrderAndProductDetailsAsync(Guid orderId)
        {
            return await _context.Orders.
                Include(o => o.ProductDetails).
                ThenInclude(pd => pd.Product).
                FirstOrDefaultAsync(o => o.Id.Equals(orderId));
        }

        public async Task<Order> GetOrderByIdWithEntriesAsync(Guid orderId)
        {
            return await _context.Orders.
                Include(o => o.Entries).
                FirstOrDefaultAsync(o => o.Id.Equals(orderId));
        }

        public async Task<PagedList<Order>> GetPagedOrdersAsync(int page, int pageSize)
        {

            return await PagedListCreator.CreateAsync(_context.Orders.Include(o => o.ProductDetails),
                page,
                pageSize);
        }

        public void Update(Order entity)
        {
            _context.Update(entity);
        }

        public void Dispose()
        {
            _context = null;
        }

        public void AddOrderDetailsToOrder(Guid orderId, OrderProductDetail orderProductDetail)
        {
            orderProductDetail.OrderId = orderId;
            _context.OrderProductDetails.Add(orderProductDetail);
        }

        public void UpdateOrderDetails(OrderProductDetail orderProductDetail)
        {
            _context.OrderProductDetails.Update(orderProductDetail);
        }

        public async Task<Order> GetOrderWithPaymentDetailsAsync(Guid orderId)
        {
            return await _context.Orders.
                Include(o => o.PaymentDetails).
                FirstOrDefaultAsync(o => o.Id.Equals(orderId));
        }

        public async Task<IEnumerable<Order>> GetAllOrdersOfUserIncludingProductDetailsAndProductsAsync(Guid userId)
        {
            return await _context.Orders.
                Include(o => o.ProductDetails).
                ThenInclude(opd => opd.Product).
                Where(o => o.OrderAuthorId.Equals(userId)).
                ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByFilterAsync(Expression<Func<Order, bool>> filter)
        {
            return await _context.Orders.Where(filter).ToListAsync();
        }

        public async Task<PagedList<Order>> GetPagedListAsync(int page, int pageSize)
        {
            return await PagedListCreator.CreateAsync(_context.Orders.
                Include(o => o.ProductDetails).
                    ThenInclude(pd => pd.Product),
                page,
                pageSize);
        }

        public async Task<PagedList<Order>> GetPagedListFilterAsync(int page, int pageSize, Expression<Func<Order, bool>> filter)
        {
            return await PagedListCreator.CreateAsync(_context.Orders.
                Include(o => o.ProductDetails).
                    ThenInclude(pd => pd.Product).Where(filter), page, pageSize);
        }

        public async Task<PagedList<Order>> GetPagedListFilterAndOrderAsync<TOrderBy>(int page, int pageSize, Expression<Func<Order, bool>> filter, Expression<Func<Order, TOrderBy>> selector)
        {
            return await PagedListCreator.CreateAsync(_context.Orders.
                Include(o => o.ProductDetails).
                    ThenInclude(pd => pd.Product).Where(filter).OrderBy(selector), page, pageSize);
        }

        public async Task<PagedList<Order>> GetPagedListFilterAndOrderDescAsync<TOrderBy>(int page, int pageSize, Expression<Func<Order, bool>> filter, Expression<Func<Order, TOrderBy>> selector)
        {
            return await PagedListCreator.CreateAsync(_context.Orders.
                Include(o => o.ProductDetails).
                    ThenInclude(pd => pd.Product).Where(filter).OrderByDescending(selector), page, pageSize);
        }

        public async Task<PagedList<Order>> GetPagedListOrderAsync<TOrderBy>(int page, int pageSize, Expression<Func<Order, TOrderBy>> selector)
        {
            return await PagedListCreator.CreateAsync(_context.Orders.
                Include(o => o.ProductDetails).
                    ThenInclude(pd => pd.Product).OrderBy(selector), page, pageSize);
        }

        public async Task<PagedList<Order>> GetPagedListOrderDescAsync<TOrderBy>(int page, int pageSize, Expression<Func<Order, TOrderBy>> selector)
        {
            return await PagedListCreator.CreateAsync(_context.Orders.
                Include(o => o.ProductDetails).
                    ThenInclude(pd => pd.Product).OrderByDescending(selector).AsQueryable(), page, pageSize);
        }
    }
}
