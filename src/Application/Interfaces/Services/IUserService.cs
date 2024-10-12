using Store.Application.DataTransferObjects;
using Store.Application.Responses;
using Store.Domain.Entities.Interfaces;

namespace Store.Application.Interfaces.Services
{
    /// <summary>
    /// Provides services for user authentication, registration, and user existence checks.
    /// </summary>
    public interface IUserService
    {
        Task<UserDto> FindUserById(Guid userId);
        Task<CreateUserResponse> RegisterAsync(UserDto userRegistrationData);
        Task<LogInResponse> LogInAsync(string login, string password);
        Task<bool> LogOutAsync();
        Task<UserDto> FindUserByLogin(string login);
        Task<string[]?> GetRolesByUserIdAsync(Guid id);
    }
}
