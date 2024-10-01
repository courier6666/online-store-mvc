using Microsoft.AspNetCore.Identity;
using Store.Application.Interfaces.IdentityManagers;
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

        public async Task<bool> CreateAsync(IUser user, string password)
        {
            return (await _userManager.CreateAsync(user as AppUser, password)).Succeeded;
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
