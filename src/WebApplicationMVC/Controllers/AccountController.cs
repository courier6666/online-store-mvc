using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Application.DataTransferObjects;
using Store.Application.Interfaces.Mapper;
using Store.Application.Interfaces.Services;
using Store.WebApplicationMVC.ViewModel;

namespace Store.WebApplicationMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "/")
        {
            return this.View(new LoginViewModel()
            {
                ReturnUrl = returnUrl,
            });
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginViewModel loginViewModel)
        {
            if (this.ModelState.IsValid)
            {
                await _userService.LogOutAsync();
                try
                {
                    var result = await _userService.LogInAsync(loginViewModel.Name, loginViewModel.Password);
                    if (result.LoginFound && result.IsPasswordCorrect)
                        return Redirect(loginViewModel.ReturnUrl);

                    if(!result.LoginFound)
                    {
                        this.ModelState.AddModelError("", "User by login not found!");
                        return View(loginViewModel);
                    }
                    if (!result.IsPasswordCorrect)
                    {
                        this.ModelState.AddModelError("", "Incorrect password for user!");
                    }
                }
                catch (Exception ex)
                {
                    this.ModelState.AddModelError("", ex.Message);
                }
                return View(loginViewModel);
            }

            return this.View(loginViewModel);
        }
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = "/")
        {
            return View(new RegistrationViewModel()
            {
                ReturnUrl = returnUrl,
            });
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm]RegistrationViewModel registrationViewModel)
        {
            var userRegistrationDto = new UserRegistrationDto()
            {
                FirstName = registrationViewModel.FirstName,
                LastName = registrationViewModel.LastName,
                Email = registrationViewModel.Email,
                Address = new AddressDto()
                {
                    Country = registrationViewModel.Country,
                    City = registrationViewModel.City,
                    State = registrationViewModel.State,
                    Street = registrationViewModel.Street,
                },
                Birthday = DateOnly.FromDateTime(registrationViewModel.Birthday.Value),
                Login = registrationViewModel.Login,
                PasswordHash = registrationViewModel.PasswordHash,
                PhoneNumber = registrationViewModel.PhoneNumber,
            };
            var registerResponse = await _userService.RegisterAsync(userRegistrationDto);
            if(registerResponse.Success)
            {
                var loginResponse = await _userService.LogInAsync(registerResponse.User.Login, registrationViewModel.PasswordHash);
                return Redirect(registrationViewModel.ReturnUrl);
            }

            return View(registrationViewModel);
        }
        public async Task<IActionResult> LogOut()
        {
            await _userService.LogOutAsync();
            return RedirectToAction("Login");
        }
    }
}
