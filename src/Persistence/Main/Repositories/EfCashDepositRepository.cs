using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities.Model;
using Store.Domain.Repositories;
using Store.Persistence.Main.DatabaseContexts;
using System.Linq.Expressions;

namespace Store.Persistence.Main.Repositories
{
    public class EfCashDepositRepository : ICashDepositRepository
    {
        public ApplicationDbContext _context;
        public EfCashDepositRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(CashDeposit entity)
        {
            _context.CashDeposits.Add(entity);
        }

        public void Delete(CashDeposit entity)
        {
            _context.CashDeposits.Remove(entity);
        }

        public void DeleteRange(IEnumerable<CashDeposit> entities)
        {
            _context.CashDeposits.RemoveRange(entities);
        }

        public void Dispose()
        {
            _context = null;
        }

        public async Task<IEnumerable<CashDeposit>> GetAllAsync()
        {
            return await _context.CashDeposits.ToListAsync();
        }

        public async Task<IEnumerable<CashDeposit>> GetByFilterAsync(Expression<Func<CashDeposit, bool>> filter)
        {
            return await _context.CashDeposits.Where(filter).ToListAsync();
        }

        public async Task<CashDeposit> GetByIdAsync(Guid id)
        {
            return await _context.CashDeposits.
                FirstOrDefaultAsync(cashDeposit => cashDeposit.Id.Equals(id));
        }


        public void Update(CashDeposit entity)
        {
            _context.CashDeposits.Update(entity);
        }
    }
}
