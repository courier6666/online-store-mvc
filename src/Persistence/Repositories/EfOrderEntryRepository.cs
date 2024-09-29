using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.Persistence.Main.DatabaseContexts;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using System.Linq.Expressions;

namespace Store.Persistence.Main.Repositories
{
    public class EfOrderEntryRepository : IOrderEntryRepository
    {
        private ApplicationDbContext _context;
        public EfOrderEntryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Entry entity)
        {
            _context.Entries.Add(entity);
        }

        public void Delete(Entry entity)
        {
            _context.Entries.Remove(entity);
        }

        public void Dispose()
        {
            _context = null;
        }

        public async Task<IEnumerable<Entry>> GetAllAsync()
        {
            return await _context.Entries.ToListAsync();
        }

        public async Task<IEnumerable<Entry>> GetAllEntriesOfOrderAsync(Guid orderId)
        {
            return await _context.Entries.
                Where(e => e.OrderId == orderId).
                ToListAsync();
        }

        public async Task<IEnumerable<Entry>> GetByFilterAsync(Expression<Func<Entry, bool>> filter)
        {
            return await _context.Entries.Where(filter).ToListAsync();
        }

        public async Task<Entry> GetByIdAsync(Guid id)
        {
            return await _context.Entries.
                FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public void Update(Entry entity)
        {
            _context.Entries.Update(entity);
        }
    }
}
