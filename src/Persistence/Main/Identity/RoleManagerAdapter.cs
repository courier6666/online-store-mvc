using Microsoft.AspNetCore.Identity;
using Store.Application.Interfaces.IdentityManagers;

namespace Store.Persistence.Main.Identity
{
    public class RoleManagerAdapter : IRoleManager
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        public RoleManagerAdapter(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<bool> CreateAsync(string roleName)
        {
            
            return (await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName))).Succeeded;
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
    }
}
