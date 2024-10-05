using Store.Domain.Entities;
using System.Linq.Expressions;

namespace Store.Domain.Repositories
{
    public interface IOrderEntryRepository : IRepository<Entry, Guid>
    {
        /// <summary>
        /// Used for getting all entries of order.
        /// </summary>
        /// <param name="orderId">Id of order <typeparamref name="Order"/></param>
        /// <returns>Collection of entries for order</returns>
        Task<IEnumerable<Entry>> GetAllEntriesOfOrderAsync(Guid orderId);
        Task<IEnumerable<Entry>> GetByFilterAsync(Expression<Func<Entry, bool>> filter);
    }
}
