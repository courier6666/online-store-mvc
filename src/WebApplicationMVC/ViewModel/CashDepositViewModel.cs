using Store.Application.DataTransferObjects;

namespace Store.WebApplicationMVC.ViewModel
{
    public class CashDepositViewModel
    {
        public CashDepositDto CashDeposit { get; set; }
        public string ReturnUrl { get; set; }
        public decimal AmountToDeposit { get; set; } = 0.00M;
    }
}
