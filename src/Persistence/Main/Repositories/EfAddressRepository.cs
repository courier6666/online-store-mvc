using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities.Model;
using Store.Domain.Repositories;
using Store.Persistence.Main.DatabaseContexts;
using System.Linq.Expressions;

namespace Store.Persistence.Main.Repositories
{
    public class EfAddressRepository : IAddressRepository
    {
        private ApplicationDbContext _context;

        public EfAddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Address entity)
        {
            _context.Addresses.Add(entity);
        }

        public void Delete(Address entity)
        {
            _context.Addresses.Remove(entity);
        }

        public void DeleteRange(IEnumerable<Address> entities)
        {
            _context.Addresses.RemoveRange(entities);
        }

        public void Dispose()
        {
            _context = null;
        }

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            return await _context.Addresses.ToListAsync();
        }

        public async Task<IEnumerable<Address>> GetByFilterAsync(Expression<Func<Address, bool>> filter)
        {
            return await _context.Addresses.Where(filter).ToListAsync();
        }

        public async Task<Address> GetByIdAsync(Guid id)
        {
            return await _context.Addresses.FirstOrDefaultAsync(a => a.Id.Equals(id));
        }

        public void Update(Address entity)
        {
            _context.Addresses.Update(entity);
        }
    }
}
