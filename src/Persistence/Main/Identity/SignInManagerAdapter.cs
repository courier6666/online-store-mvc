using Microsoft.AspNetCore.Identity;
using Store.Application.Interfaces.IdentityManagers;
using Store.Domain.Entities.Interfaces;

namespace Store.Persistence.Main.Identity
{
    public class SignInManagerAdapter : ISignInManager
    {
        private readonly SignInManager<AppUser> _signInManager;
        public SignInManagerAdapter(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public async Task<bool> SignInPasswordAsync(IUser user, string password)
        {
            return (await _signInManager.PasswordSignInAsync(user as AppUser, password, false, false)).Succeeded;
        }
        public async Task SignInAsync(IUser user)
        {
            await _signInManager.SignInAsync(user as AppUser, false);
        }
        public async Task<bool> SignOutAsync()
        {
            await _signInManager.SignOutAsync();
            return true;
        }
    }
}
