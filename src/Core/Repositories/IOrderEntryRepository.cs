using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Store.Domain.Entities;

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
