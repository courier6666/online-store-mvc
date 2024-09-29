using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Domain.Entities.Interfaces;

namespace Store.Domain.Entities.Model
{
    /// <summary>
    /// Allows to change information of cash deposits balance. Used for properly changing cash deposit balance.
    /// </summary>
    public abstract class CashDeposit : Entity<Guid>, ICashDeposit
    {
        public decimal CurrentMoneyBalance { get; protected set; }
        /// <summary>
        /// Init property used for setting current balance of cash deposit.
        /// Used while creating cash deposits.
        /// </summary>
        public decimal CurrentMoneyBalanceSetter
        {
            init
            {
                CurrentMoneyBalance = value;
            }
        }

        public abstract void DepositAmount(decimal moneyAmount);
        public abstract void WithdrawAmount(decimal moneyAmount);
        public Guid UserId { get; set; }
        public IUser User { get; set; }
        /// <summary>
        /// All payment details that contain this cash deposit.
        /// </summary>
        /// <returns>Collections of all payment details.</returns>
        public virtual ICollection<PaymentDetails> PaymentDetails { get; protected set; }
    }
}
