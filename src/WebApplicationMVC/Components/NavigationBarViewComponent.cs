using Microsoft.AspNetCore.Mvc;
using Store.Application.Interfaces.Services;
using Store.Domain.Entities.Interfaces;
using Store.Domain.Entities.Model;
using Store.WebApplicationMVC.ViewModel;

namespace Store.WebApplicationMVC.Components
{
    [ViewComponent]
    public class NavigationBarViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IUserContext _userContext;
        private readonly ICashDepositService _cashDepositService;
        public NavigationBarViewComponent(IProductService productService, IUserService userService, IUserContext userContext, ICashDepositService cashDepositService)
        {
            _productService = productService;
            _userService = userService;
            _userContext = userContext;
            _cashDepositService = cashDepositService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            var cashDeposit = !_userContext.IsAuthenticated ? null : (await _cashDepositService.GetAllCashDepositsForUser(_userContext.UserId.Value)).First();
            return View(new NavBarViewModel()
            {
                UserContext = _userContext,
                CashDeposit = cashDeposit,
                IsUserAdmin = _userContext.IsAuthenticated &&
                    _userContext.UserId != null &&
                    ((await _userService.GetRolesByUserIdAsync(_userContext.UserId.Value))?.Contains(Roles.Admin) ?? false)
            });
        }
    }
}
