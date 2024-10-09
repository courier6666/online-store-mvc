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
        public NavigationBarViewComponent(IProductService productService, IUserService userService, IUserContext userContext)
        {
            _productService = productService;
            _userService = userService;
            _userContext = userContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View(new NavBarViewModel()
            {
                UserContext = _userContext,
                IsUserAdmin = _userContext.IsAuthenticated &&
                    _userContext.UserId != null &&
                    ((await _userService.GetRolesByUserIdAsync(_userContext.UserId.Value))?.Contains(Roles.Admin) ?? false)
            });
        }
    }
}
