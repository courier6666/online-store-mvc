using Store.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Interfaces.IdentityManagers
{
    public interface IUserManager
    {
        Task<IUser> FindByNameAsync(string username);
        Task<bool> CreateAsync(IUser user, string password);
        Task<IList<string>> GetRolesAsync(IUser user);
        Task AddToRoleAsync(IUser user, string roleName);
        Task<bool> CheckPasswordAsync(IUser user, string password);
    }
}
