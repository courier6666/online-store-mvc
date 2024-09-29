using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Application.DataTransferObjects;
using Store.Application.Responses;

namespace Store.Application.Interfaces.Services
{
    /// <summary>
    /// Provides services for user authentication, registration, and user existence checks.
    /// </summary>
    public interface IUserService
    {
        Task<Guid> RegisterAsync(UserRegistrationDto userRegistrationData);
        Task<LogInResponse> LogInAsync(string login, string password);
        Task<bool> UserWithLoginExists(string login);
    }
}
