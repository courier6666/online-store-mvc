using Store.Domain.Entities.Model;
using System.Linq.Expressions;

namespace Store.Domain.Repositories
{
    public interface IAddressRepository : IRepository<Address, Guid>
    {
        Task<IEnumerable<Address>> GetByFilterAsync(Expression<Func<Address, bool>> filter);
    }
}
