using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.Persistence.Main.DatabaseContexts;
using Store.Domain.Entities.Model;
using Store.Domain.Repositories;
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
