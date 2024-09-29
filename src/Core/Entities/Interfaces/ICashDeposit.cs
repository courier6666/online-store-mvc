namespace Store.Domain.Entities.Interfaces;

/// <summary>
/// Allows to change information of cash deposits balance. Used for properly changing cash deposit balance.
/// </summary>
public interface ICashDeposit
{
    /// <summary>
    /// Used for viewing current cash deposit balance.
    /// </summary>
    /// <returns>Current balance on cash deposit.</returns>
    public decimal CurrentMoneyBalance { get; }
    /// <summary>
    /// Used for depositing amount of money to cash deposit.
    /// </summary>
    /// <param name="moneyAmount"> Amount of money to deposit to cash deposit.</param>
    public void DepositAmount(decimal moneyAmount);
    /// <summary>
    /// Used for withdrawing money from cash deposit.
    /// </summary>
    /// <param name="moneyAmount"> Amount of money to withdraw from cash deposit.</param>
    public void WithdrawAmount(decimal moneyAmount);

}