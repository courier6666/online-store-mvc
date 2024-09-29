using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Domain.Repositories;

namespace Store.Domain.Entities.Interfaces
{
    /// <summary>
    /// UnitOfWork pattern. Used for carrying out database transactions.
    /// Contains all repositories and methods for beginning, commiting and rolling back transaction.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void Rollback();
        Task CommitAsync();
        ICashDepositRepository CashDepositRepository { get; }
        IOrderEntryRepository OrderEntryRepository { get; }
        IOrderRepository OrderRepository { get; }
        IProductRepository ProductRepository { get; }
        IUserRepository UserRepository { get; }
        IPaymentDetailsRepository PaymentDetailsRepository { get; }
        IAddressRepository AddressRepository { get; }
    }

}
