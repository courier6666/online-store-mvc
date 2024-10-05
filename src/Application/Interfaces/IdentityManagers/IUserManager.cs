using Store.Application.Responses;
using Store.Domain.Entities.Interfaces;

namespace Store.Application.Interfaces.IdentityManagers
{
    public interface IUserManager
    {
        Task<IUser> FindByNameAsync(string username);
        Task<CreateUserResponse> CreateAsync(IUser user, string password);
        Task<IList<string>> GetRolesAsync(IUser user);
        Task AddToRoleAsync(IUser user, string roleName);
        Task<bool> CheckPasswordAsync(IUser user, string password);
    }
}
