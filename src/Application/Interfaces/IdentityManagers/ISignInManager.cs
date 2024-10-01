using Store.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Interfaces.IdentityManagers
{
    public interface ISignInManager
    {
        Task<bool> SignOutAsync();
        Task<bool> SignInPasswordAsync(IUser user, string password);
        Task SignInAsync(IUser user);
    }
}
