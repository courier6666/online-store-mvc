using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Application.DataTransferObjects;
using Store.Domain.Entities.Model;

namespace Store.Application.Interfaces.Services
{
    /// <summary>
    /// Provides services for managing cash deposits including deposit, withdrawal,
    /// opening cash deposits for users and administrators, and retrieving all cash deposits for a user.
    /// </summary>
    public interface ICashDepositService : ICrudService<CashDepositDto>
    {
        Task<Guid> OpenCashDepositForUserAsync(Guid userId);
        Task<Guid> OpenCashDepositForAdminAsync(Guid adminId);
        Task DepositAsync(Guid userId, Guid cashDepositId, decimal amount);
        Task WithdrawAsync(Guid userId, Guid cashDepositId, decimal amount);
        Task<IEnumerable<CashDepositDto>> GetAllCashDepositsForUser(Guid userId);
    }
}
