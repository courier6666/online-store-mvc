namespace Store.Application.Interfaces.IdentityManagers
{
    public interface IRoleManager
    {
        Task<bool> RoleExistsAsync(string roleName);
        Task<bool> CreateAsync(string roleName);
    }
}
