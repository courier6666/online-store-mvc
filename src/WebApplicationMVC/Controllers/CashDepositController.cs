using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Interfaces.Services;
using Store.Domain.Entities.Interfaces;
using Store.WebApplicationMVC.ViewModel;

namespace Store.WebApplicationMVC.Controllers
{
    [Authorize]
    public class CashDepositController : Controller
    {
        private readonly ICashDepositService _cashDepositService;
        private readonly IUserContext _userContext;
        public CashDepositController(ICashDepositService cashDepositService, IUserContext userContext)
        {
            _cashDepositService = cashDepositService;
            _userContext = userContext;
        }
        public async Task<IActionResult> Deposit(Guid cashDepositId, string returnUrl = "/")
        {
            var cashDeposit = await _cashDepositService.GetByIdAsync(cashDepositId);
            return View(new CashDepositViewModel()
            {
                CashDeposit = cashDeposit,
                ReturnUrl = returnUrl
            });
        }
        [HttpPost]
        public async Task<IActionResult> Deposit([FromForm] CashDepositViewModel cashDepositViewModel)
        {
            await _cashDepositService.DepositAsync(_userContext.UserId.Value, cashDepositViewModel.CashDeposit.Id, cashDepositViewModel.AmountToDeposit);
            return Redirect(cashDepositViewModel.ReturnUrl);
        }
    }
}
