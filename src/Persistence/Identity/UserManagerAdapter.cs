using Microsoft.AspNetCore.Identity;
using Store.Application.Interfaces.IdentityManagers;
using Store.Application.Responses;
using Store.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.Main.Identity
{
    public class UserManagerAdapter : IUserManager
    {
        private readonly UserManager<AppUser> _userManager;
        public UserManagerAdapter(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task AddToRoleAsync(IUser user, string roleName)
        {
            await _userManager.AddToRoleAsync(user as AppUser, roleName);
        }

        public async Task<bool> CheckPasswordAsync(IUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user as AppUser, password);
        }

        public async Task<CreateUserResponse> CreateAsync(IUser user, string password)
        {
            var result = await _userManager.CreateAsync(user as AppUser, password);
            return new CreateUserResponse()
            {
                Success = result.Succeeded,
                Errors = result.Succeeded ? null : result.Errors.Select(e => e.Description).ToArray(),
            };
        }
        public async Task<IUser> FindByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<IList<string>> GetRolesAsync(IUser user)
        {
            return await _userManager.GetRolesAsync(user as AppUser);
        }
    }
}
