namespace Store.Domain.Entities.Model;

public class AdministratorCashDeposit : CashDeposit
{
    public AdministratorCashDeposit()
    {
        CurrentMoneyBalance = decimal.Zero;
    }
    /// <summary>
    /// Used for depositing money into cash deposit.
    /// </summary>
    /// <param name="moneyAmount"></param>
    public override void DepositAmount(decimal moneyAmount)
    {

    }
    /// <summary>
    /// Used for withdrawing money from administrator cash deposit.
    /// No exception thrown when overdrawing money, because administrator has limitless cash balance.
    /// </summary>
    /// <param name="moneyAmount"></param>
    public override void WithdrawAmount(decimal moneyAmount)
    {

    }
}