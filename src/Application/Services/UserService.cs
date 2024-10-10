using Store.Application.DataTransferObjects;
using Store.Application.Factories;
using Store.Application.Interfaces.IdentityManagers;
using Store.Application.Interfaces.Mapper;
using Store.Application.Interfaces.Services;
using Store.Application.Responses;
using Store.Domain.Entities.Interfaces;
using Store.Domain.Entities.Model;

namespace Store.Application.Services
{
    /// <summary>
    /// Provides services for user authentication, registration, and user existence checks.
    /// </summary>
    public sealed class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomMapper _customMapper;
        private readonly IUserFactory _userFactory;
        private readonly IUserManager _userManager;
        private readonly ISignInManager _signInManager;
        private readonly IRoleManager _roleManager;
        public UserService(IUnitOfWork unitOfWork, ICustomMapper customMapper, IUserFactory userFactory,
            IUserManager userManager, ISignInManager signInManager, IRoleManager roleManager)
        {
            _unitOfWork = unitOfWork;
            _customMapper = customMapper;
            _userFactory = userFactory;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        /// <summary>
        /// Authenticates a user by their login and password.
        /// </summary>
        /// <param name="login">The user's login.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>A <see cref="LogInResponse"/> indicating the result of the login attempt.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the operation to find the user by login fails.</exception>
        public async Task<LogInResponse> LogInAsync(string login, string password)
        {
            try
            {
                var foundUser = await _userManager.FindByNameAsync(login);

                if (foundUser == null)
                    return new LogInResponse() { UserId = null, LoginFound = false, IsPasswordCorrect = false };

                if (!(await _userManager.CheckPasswordAsync(foundUser, password)))
                    return new LogInResponse() { UserId = null, LoginFound = true, IsPasswordCorrect = false };

                var result = await _signInManager.SignInPasswordAsync(foundUser, password);
                if (!result)
                {
                    return new LogInResponse() { SignedIn = false, LoginFound = true, IsPasswordCorrect = true };
                }

                return new LogInResponse() { UserId = foundUser.Id, SignedIn = true, LoginFound = true, IsPasswordCorrect = true, UserRoles = (await _userManager.GetRolesAsync(foundUser)).ToArray() };

            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Operation to sign in failed!", innerException: e);
            }

        }

        public async Task<bool> LogOutAsync()
        {
            try
            {
                return await _signInManager.SignOutAsync();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException("Operation to log out failed!", innerException: e);
            }
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userRegistrationData">The data for the new user.</param>
        /// <returns>The unique identifier of the newly registered user.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the operation to register a new user fails.</exception>
        public async Task<CreateUserResponse> RegisterAsync(UserRegistrationDto userRegistrationData)
        {
            IUser newUser = _userFactory.CreateNewEmptyUser();
            _customMapper.MapToExisting(userRegistrationData, ref newUser);
            try
            {
                var response = await _userManager.CreateAsync(newUser, userRegistrationData.PasswordHash);
                await _userManager.AddToRoleAsync(newUser, Roles.User);
                if (response.Success)
                    response.User = newUser;

                return response;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Operation to register new user failed!", innerException: e);
            }

        }
        /// <summary>
        /// Checks if a user with the specified login exists.
        /// </summary>
        /// <param name="login">The login to check.</param>
        /// <returns><c>true</c> if a user with the specified login exists; otherwise, <c>false</c>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the operation to find the user by login fails.</exception>
        public async Task<IUser> FindUserByLogin(string login)
        {
            try
            {
                var foundUser = await _userManager.FindByNameAsync(login);
                return foundUser;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Operation to find user by login failed!", innerException: e);
            }
        }

        public async Task<string[]?> GetRolesByUserIdAsync(Guid id)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var foundUser = await _unitOfWork.UserRepository.GetByIdAsync(id);
                var roles = (await _userManager.GetRolesAsync(foundUser)).ToArray();
                await _unitOfWork.CommitAsync();
                return roles.Count() > 0 ? roles : null;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException("Operation to get roles of user!", innerException: e);
            }
        }

        public async Task<IUser> FindUserById(Guid userId)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var foundUser = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                var roles = (await _userManager.GetRolesAsync(foundUser)).ToArray();
                await _unitOfWork.CommitAsync();
                return foundUser;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException("Operation to get roles of user!", innerException: e);
            }
        }
    }
}
