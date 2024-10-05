using Store.Domain.Entities.Model;
using System.Linq.Expressions;

namespace Store.Domain.Repositories
{
    public interface IPaymentDetailsRepository : IRepository<PaymentDetails, Guid>
    {
        Task<IEnumerable<PaymentDetails>> GetByFilterAsync(Expression<Func<PaymentDetails, bool>> filter);
    }
}
