using Store.Domain.Entities.Interfaces;

namespace Store.Application.Interfaces.IdentityManagers
{
    public interface ISignInManager
    {
        Task<bool> SignOutAsync();
        Task<bool> SignInPasswordAsync(IUser user, string password);
        Task SignInAsync(IUser user);
    }
}
