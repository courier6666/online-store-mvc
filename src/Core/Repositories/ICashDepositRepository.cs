using Store.Domain.Entities.Model;
using System.Linq.Expressions;

namespace Store.Domain.Repositories
{
    public interface ICashDepositRepository : IRepository<CashDeposit, Guid>
    {
        Task<IEnumerable<CashDeposit>> GetByFilterAsync(Expression<Func<CashDeposit, bool>> filter);
    }
}
