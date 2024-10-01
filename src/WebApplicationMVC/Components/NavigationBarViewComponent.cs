using Microsoft.AspNetCore.Mvc;
using Store.Application.Interfaces.Services;
using Store.Domain.Entities.Interfaces;
using Store.WebApplicationMVC.ViewModel;

namespace Store.WebApplicationMVC.Components
{
    [ViewComponent]
    public class NavigationBarViewComponent : ViewComponent
    {
        private readonly IProductService _productService;
        private readonly IUserContext _userContext;
        public NavigationBarViewComponent(IProductService productService, IUserContext userContext)
        {
            _productService = productService;
            _userContext = userContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(new NavBarViewModel()
            {
                UserContext = _userContext,
            });
        }
    }
}
