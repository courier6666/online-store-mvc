namespace Store.Application.DataTransferObjects
{
    public class CashDepositDto
    {
        public Guid Id { get; set; }
        public decimal CurrentMoneyBalance { get; set; }
        public Guid UserId { get; set; }
    }
}
