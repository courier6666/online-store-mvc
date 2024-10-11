using Store.Application.DataTransferObjects;
using Store.Domain.Entities.Interfaces;

namespace Store.WebApplicationMVC.ViewModel
{
    public class NavBarViewModel
    {
        public IUserContext UserContext { get; set; }
        public CashDepositDto CashDeposit { get; set; }
        public bool IsUserAdmin { get; init; }
    }
}
