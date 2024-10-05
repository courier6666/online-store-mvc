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
        Task<CreateUserResponse> RegisterAsync(UserRegistrationDto userRegistrationData);
        Task<LogInResponse> LogInAsync(string login, string password);
        Task<bool> LogOutAsync();
        Task<IUser> FindUserByLogin(string login);
    }
}
