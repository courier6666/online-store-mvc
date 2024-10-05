using System.Globalization;

namespace Store.Domain.Entities.Model
{
    public class UserCashDeposit : CashDeposit
    {
        /// <summary>
        /// Used for depositing money into cash deposit.
        /// </summary>
        /// <param name="moneyAmount"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public override void DepositAmount(decimal moneyAmount)
        {
            if (moneyAmount < 0m)
                throw new ArgumentOutOfRangeException(nameof(moneyAmount), $"Amount of money to deposit must not be negative! Provided money amount: {moneyAmount.ToString("C", new CultureInfo("uk-UA", false))}");

            CurrentMoneyBalance += moneyAmount;
        }
        /// <summary>
        /// Used for withdrawing money from cash deposit.
        /// </summary>
        /// <param name="moneyAmount"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public override void WithdrawAmount(decimal moneyAmount)
        {
            if (moneyAmount < 0m)
                throw new ArgumentOutOfRangeException(nameof(moneyAmount), $"Amount of money to deposit must not be negative! Provided money amount: {moneyAmount.ToString("C", new CultureInfo("uk-UA", false))}");

            if (moneyAmount > CurrentMoneyBalance)
                throw new InvalidOperationException($"Cannot withdraw more money from deposit that its current balance! Current balance: {CurrentMoneyBalance.ToString("C", new CultureInfo("uk-UA", false))}, Amount to withdraw: {moneyAmount.ToString("C", new CultureInfo("uk-UA", false))}");

            CurrentMoneyBalance -= moneyAmount;
        }
    }
}
