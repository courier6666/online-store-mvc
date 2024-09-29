using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Store.Domain.Entities.Model;

namespace Store.Domain.Repositories
{
    public interface IPaymentDetailsRepository : IRepository<PaymentDetails, Guid>
    {
        Task<IEnumerable<PaymentDetails>> GetByFilterAsync(Expression<Func<PaymentDetails, bool>> filter);
    }
}
