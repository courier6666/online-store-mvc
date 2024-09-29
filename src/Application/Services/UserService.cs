using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Application.Interfaces.Mapper;
using Store.Application.DataTransferObjects;
using Store.Application.Interfaces.Services;
using Store.Application.Responses;
using Store.Domain.Entities;
using Store.Domain.Entities.Interfaces;
using Store.Domain.Entities.Model;
using Store.Application.Factories;

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
        public UserService(IUnitOfWork unitOfWork, ICustomMapper customMapper, IUserFactory userFactory)
        {
            _unitOfWork = unitOfWork;
            _customMapper = customMapper;
            _userFactory = userFactory;
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
                _unitOfWork.BeginTransaction();
                var foundUser = await _unitOfWork.UserRepository.FindUserByLoginAsync(login);
                await _unitOfWork.CommitAsync();

                if (foundUser == null)
                    return new LogInResponse() { UserId = null, LoginFound = false, IsPasswordCorrect = false };

                if (!foundUser.PasswordHash.Equals(password))
                    return new LogInResponse() { UserId = null, LoginFound = true, IsPasswordCorrect = false };

                string[] userRoles = foundUser.Roles.ToString().Replace(" ", "").Split(",");

                return new LogInResponse() { UserId = foundUser.Id, LoginFound = true, IsPasswordCorrect = true, UserRoles = userRoles };

            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException("Operation to find user by login failed!", innerException: e);
            }

        }
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userRegistrationData">The data for the new user.</param>
        /// <returns>The unique identifier of the newly registered user.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the operation to register a new user fails.</exception>
        public async Task<Guid> RegisterAsync(UserRegistrationDto userRegistrationData)
        {

            _unitOfWork.BeginTransaction();
            IUser newUser = _userFactory.CreateNewEmptyUser();
            _customMapper.MapToExisting(userRegistrationData, ref newUser);
            newUser.Roles = new[] { Roles.User }.ToList();
            try
            {
                _unitOfWork.UserRepository.Add(newUser);
                await _unitOfWork.CommitAsync();
                return newUser.Id;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException("Operation to register new user failed!", innerException: e);
            }

        }
        /// <summary>
        /// Checks if a user with the specified login exists.
        /// </summary>
        /// <param name="login">The login to check.</param>
        /// <returns><c>true</c> if a user with the specified login exists; otherwise, <c>false</c>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the operation to find the user by login fails.</exception>
        public async Task<bool> UserWithLoginExists(string login)
        {

            bool result;
            try
            {
                _unitOfWork.BeginTransaction();
                result = (await _unitOfWork.UserRepository.FindUserByLoginAsync(login)) != null;
                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException("Operation to find user by login failed!", innerException: e);
            }

            return result;

        }
    }
}
