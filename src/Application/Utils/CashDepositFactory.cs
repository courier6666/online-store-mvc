using Store.Domain.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Utils
{
    public static class CashDepositFactory
    {
        public static CashDeposit CreateEmptyUserCashDeposit(Guid userId)
        {
            return new UserCashDeposit()
            {
                UserId = userId,
                CurrentMoneyBalanceSetter = 0.00M
            };
        }
        public static CashDeposit CreateEmptyAdminCashDeposit(Guid adminId)
        {
            return new AdministratorCashDeposit()
            {
                UserId = adminId,
                CurrentMoneyBalanceSetter = 0.00M
            };
        }
    }
}
