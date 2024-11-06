using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities.Model;
using Store.Domain.Repositories;
using Store.Persistence.Main.DatabaseContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.Main.Repositories
{
    public class EfFavouriteProductsRepository : IFavouriteProductsRepository
    {
        private ApplicationDbContext _context;
        public EfFavouriteProductsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(FavouriteProduct entity)
        {
            _context.Add(entity);
        }

        public async Task<Guid[]> AreTheseFavoruiteProductsOfUserAsync(Guid userId, Guid[] productIds)
        {
            return await _context.FavouriteProducts.
                AsNoTracking().
                Where(f => f.UserId == userId && productIds.Contains(f.ProductId)).
                Select(f => f.ProductId).
                ToArrayAsync();
        }

        public void Delete(FavouriteProduct entity)
        {
            _context.Remove(entity);
        }

        public void DeleteRange(IEnumerable<FavouriteProduct> entities)
        {
            _context.RemoveRange(entities);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public async Task<IEnumerable<FavouriteProduct>> GetAllAsync()
        {
            return _context.FavouriteProducts.ToList();
        }

        public async Task<FavouriteProduct> GetByIdAsync((Guid userId, Guid productId) id)
        {
            return await _context.FavouriteProducts.FirstOrDefaultAsync(f => f.UserId == id.userId && f.ProductId == id.productId);
        }

        public void Update(FavouriteProduct entity)
        {
            _context.Update(entity);
        }
    }
}
