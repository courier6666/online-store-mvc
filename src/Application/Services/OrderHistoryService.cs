using Store.Application.DataTransferObjects;
using Store.Application.Interfaces.Mapper;
using Store.Application.Interfaces.Services;
using Store.Domain.Entities;
using Store.Domain.Entities.Interfaces;

namespace Store.Application.Services
{
    /// <summary>
    /// Provides services for managing order history that consists of entries.
    /// Contains operations to retrieve logging history of order.
    /// </summary>
    public class OrderHistoryService : IOrderHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomMapper _customMapper;

        public OrderHistoryService(IUnitOfWork unitOfWork, ICustomMapper customMapper)
        {
            _unitOfWork = unitOfWork;
            _customMapper = customMapper;
        }

        public async Task<Guid> AddAsync(EntryDto model)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                Entry entry = _customMapper.Map<EntryDto, Entry>(model);
                _unitOfWork.OrderEntryRepository.Add(entry);
                await _unitOfWork.CommitAsync();
                return entry.Id;
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
                _unitOfWork.OrderEntryRepository.Delete(await _unitOfWork.OrderEntryRepository.GetByIdAsync(modelId));
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }

        public async Task<IEnumerable<EntryDto>> GetAllAsync()
        {
            return _customMapper.MapEnumerable<Entry, EntryDto>(await _unitOfWork.OrderEntryRepository.GetAllAsync());
        }

        public async Task<EntryDto> GetByIdAsync(Guid id)
        {
            var entry = _customMapper.Map<Entry, EntryDto>(await _unitOfWork.OrderEntryRepository.GetByIdAsync(id));
            return entry;
        }

        /// <summary>
        /// Retrieves all entries for order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<IEnumerable<EntryDto>> GetOrderHistoryAsync(Guid orderId)
        {
            try
            {
                IEnumerable<Entry> entries = await _unitOfWork.OrderEntryRepository.GetAllEntriesOfOrderAsync(orderId);
                return _customMapper.MapEnumerable<Entry, EntryDto>(entries);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException($"Failed to get order history!", innerException: ex);
            }
        }

        public async Task UpdateAsync(EntryDto model)
        {
            try
            {
                if (model.Id == null)
                    throw new ArgumentException("CashDeposit id is null!");

                _unitOfWork.BeginTransaction();
                Entry entry = await _unitOfWork.OrderEntryRepository.GetByIdAsync(model.Id.Value);

                _customMapper.MapToExisting(model, ref entry);

                _unitOfWork.OrderEntryRepository.Update(entry);
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
