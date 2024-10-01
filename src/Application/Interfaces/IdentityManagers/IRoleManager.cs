using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Interfaces.IdentityManagers
{
    public interface IRoleManager
    {
        Task<bool> RoleExistsAsync(string roleName);
        Task<bool> CreateAsync(string roleName);
    }
}
