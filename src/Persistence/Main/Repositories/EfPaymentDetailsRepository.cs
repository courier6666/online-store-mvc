using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities.Model;
using Store.Domain.Repositories;
using Store.Persistence.Main.DatabaseContexts;
using System.Linq.Expressions;

namespace Store.Persistence.Main.Repositories
{
    public class EfPaymentDetailsRepository : IPaymentDetailsRepository
    {
        private ApplicationDbContext _context;

        public EfPaymentDetailsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(PaymentDetails entity)
        {
            _context.PaymentDetails.Add(entity);
        }

        public void Delete(PaymentDetails entity)
        {
            _context.PaymentDetails.Remove(entity);
        }

        public void Dispose()
        {
            _context = null;
        }

        public async Task<IEnumerable<PaymentDetails>> GetAllAsync()
        {
            return await _context.PaymentDetails.ToListAsync();
        }

        public async Task<PaymentDetails> GetByIdAsync(Guid id)
        {
            return await _context.PaymentDetails.FirstOrDefaultAsync(p => p.Id.Equals(id));
        }

        public void Update(PaymentDetails entity)
        {
            _context.PaymentDetails.Update(entity);
        }

        public async Task<PaymentDetails> GetPaymentDetailsWithOrderAndCashDeposit(Guid id)
        {
            return await _context.PaymentDetails.Include(p => p.CashDeposit).Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id.Equals(id));
        }

        public async Task<IEnumerable<PaymentDetails>> GetByFilterAsync(Expression<Func<PaymentDetails, bool>> filter)
        {
            return await _context.PaymentDetails.Where(filter).ToListAsync();
        }
    }
}
