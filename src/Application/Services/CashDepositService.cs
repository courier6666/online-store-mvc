using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Application.Interfaces.Mapper;
using Store.Application.DataTransferObjects;
using Store.Application.Interfaces.Services;
using Store.Domain.Entities;
using Store.Domain.Entities.Interfaces;
using Store.Domain.Entities.Model;
using System.Reflection;

namespace Store.Application.Services
{
    /// <summary>
    /// Provides services for managing cash deposits including deposit, withdrawal,
    /// opening cash deposits for users and administrators, and retrieving all cash deposits for a user.
    /// </summary>
    public class CashDepositService : ICashDepositService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomMapper _customMapper;

        public CashDepositService(IUnitOfWork unitOfWork, ICustomMapper customMapper)
        {
            _unitOfWork = unitOfWork;
            _customMapper = customMapper;
        }
        /// <summary>
        /// Deposits specified amount of money to cash deposit balance.
        /// </summary>
        /// <param name="userId">User</param>
        /// <param name="cashDepositId">Cash deposit of user</param>
        /// <param name="amount">Amount to deposit</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when user or cash deposit is not found;Cash Deposit owner id is different from found user id 
        /// </exception>
        public async Task DepositAsync(Guid userId, Guid cashDepositId, decimal amount)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                IUser foundUser = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (foundUser == null)
                {
                    throw new InvalidOperationException($"User by id '{userId}' not found.");
                }

                CashDeposit foundCashDeposit = await _unitOfWork.CashDepositRepository.GetByIdAsync(cashDepositId);
                if (foundCashDeposit == null)
                {
                    throw new InvalidOperationException($"Cash deposit by id '{cashDepositId}' not found.");
                }

                if (foundCashDeposit.UserId != foundUser.Id)
                {
                    throw new InvalidOperationException(
                        $"Cash deposit by id '{cashDepositId}' does not belong to user {userId}.");
                }

                foundCashDeposit.DepositAmount(amount);
                _unitOfWork.CashDepositRepository.Update(foundCashDeposit);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }
        /// <summary>
        /// Gets all cash deposit for specified user.
        /// </summary>
        /// <param name="userId">User</param>
        /// <returns>All cash deposits of specified user.</returns>
        public async Task<IEnumerable<CashDepositDto>> GetAllCashDepositsForUser(Guid userId)
        {

            var cashDeposits = await _unitOfWork.CashDepositRepository.GetAllAsync();
            return _customMapper.MapEnumerable<CashDeposit, CashDepositDto>(cashDeposits);
        }
        /// <summary>
        /// Opens, creates a new cash deposit for user.
        /// </summary>
        /// <param name="userId">User</param>
        /// <returns>Id of newly created cash deposit.</returns>
        /// <exception cref="InvalidOperationException">Thrown when user is not found</exception>
        public async Task<Guid> OpenCashDepositForUserAsync(Guid userId)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                IUser foundUser = await _unitOfWork.UserRepository.GetByIdAsync(userId);

                if (foundUser != null)
                    throw new InvalidOperationException($"User with id '{userId}' not found.");

                UserCashDeposit newCashDeposit = new UserCashDeposit()
                {
                    CurrentMoneyBalanceSetter = 0.00m,
                    UserId = userId
                };

                _unitOfWork.CashDepositRepository.Add(newCashDeposit);
                await _unitOfWork.CommitAsync();
                return newCashDeposit.Id;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }
        /// <summary>
        /// Opens, creates a new cash deposit for admin.
        /// </summary>
        /// <param name="adminId">Admin</param>
        /// <returns>id of newly created cash deposit.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when admin is not found; found user is not an admin
        /// </exception>
        public async Task<Guid> OpenCashDepositForAdminAsync(Guid adminId)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                IUser foundUser = await _unitOfWork.UserRepository.GetByIdAsync(adminId);

                if (foundUser != null)
                    throw new InvalidOperationException($"User with id '{adminId}' not found.");

                if (!foundUser.Roles.Contains(Roles.Admin))
                    throw new InvalidOperationException($"User with id '{adminId}' is not an administrator.");

                AdministratorCashDeposit newCashDeposit = new AdministratorCashDeposit()
                {
                    CurrentMoneyBalanceSetter = 0.00m,
                    UserId = adminId
                };

                _unitOfWork.CashDepositRepository.Add(newCashDeposit);
                await _unitOfWork.CommitAsync();
                return newCashDeposit.Id;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }
        /// <summary>
        /// Withdraws specified amount of money from cash deposit balance.
        /// </summary>
        /// <param name="userId">User</param>
        /// <param name="cashDepositId">Cash deposit of user</param>
        /// <param name="amount">Amount to deposit</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when user or cash deposit is not found;cash deposit owner id is different from found user id
        /// </exception>
        public async Task WithdrawAsync(Guid userId, Guid cashDepositId, decimal amount)
        {
            try
            {
                IUser foundUser = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (foundUser == null)
                {
                    throw new InvalidOperationException($"User by id '{userId}' not found.");
                }

                CashDeposit foundCashDeposit = await _unitOfWork.CashDepositRepository.GetByIdAsync(cashDepositId);
                if (foundCashDeposit == null)
                {
                    throw new InvalidOperationException($"Cash deposit by id '{cashDepositId}' not found.");
                }

                if (foundCashDeposit.UserId != foundUser.Id)
                {
                    throw new InvalidOperationException(
                        $"Cash deposit by id '{cashDepositId}' does not belong to user {userId}.");
                }

                foundCashDeposit.WithdrawAmount(amount);
                _unitOfWork.CashDepositRepository.Update(foundCashDeposit);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }

        public async Task<IEnumerable<CashDepositDto>> GetAllAsync()
        {
            var cashDeposits = _customMapper.MapEnumerable<CashDeposit, CashDepositDto>(await _unitOfWork.CashDepositRepository.GetAllAsync());
            return cashDeposits;

        }
        public async Task<CashDepositDto> GetByIdAsync(Guid id)
        {
            var cashDeposit = _customMapper.Map<CashDeposit, CashDepositDto>(await _unitOfWork.CashDepositRepository.GetByIdAsync(id));
            return cashDeposit;
        }

        public async Task<Guid> AddAsync(CashDepositDto model)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                CashDeposit cashDeposit = _customMapper.Map<CashDepositDto, UserCashDeposit>(model);
                _unitOfWork.CashDepositRepository.Add(cashDeposit);
                await _unitOfWork.CommitAsync();
                return cashDeposit.Id;
            }
            catch(Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }

        public async Task UpdateAsync(CashDepositDto model)
        {
            try
            {
                if (model.Id == null)
                    throw new ArgumentException("CashDeposit id is null!");

                _unitOfWork.BeginTransaction();
                CashDeposit cashDeposit = await _unitOfWork.CashDepositRepository.GetByIdAsync(model.Id);

                _customMapper.MapToExisting(model, ref cashDeposit);

                _unitOfWork.CashDepositRepository.Update(cashDeposit);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }

        public async Task DeleteAsync(Guid modelId)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _unitOfWork.CashDepositRepository.Delete(await _unitOfWork.CashDepositRepository.GetByIdAsync(modelId));
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }
    }
}
