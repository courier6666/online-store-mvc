using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.PagedLists;
using Store.Domain.Repositories;
using Store.Persistence.Main.DatabaseContexts;
using Store.Persistence.Main.PagedListCreatorNm;
using System.Linq.Expressions;

namespace Store.Persistence.Main.Repositories
{
    public class EfProductRepository : IProductRepository
    {
        private ApplicationDbContext _context;
        public EfProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Product entity)
        {
            _context.Products.Add(entity);
        }

        public void Delete(Product entity)
        {
            _context.Products.Remove(entity);
        }

        public void Dispose()
        {
            _context = null;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByFilterAsync(Expression<Func<Product, bool>> filter)
        {
            return await _context.Products.Where(filter).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await _context.Products.
                FirstOrDefaultAsync(p => p.Id.Equals(id));
        }

        public void Update(Product entity)
        {
            _context.Products.Update(entity);
        }
        public async Task<PagedList<Product>> GetPagedListAsync(int page, int pageSize)
        {
            return await PagedListCreator.CreateAsync(_context.Products, page, pageSize);
        }

        public async Task<PagedList<Product>> GetPagedListFilterAsync(int page, int pageSize, Expression<Func<Product, bool>> filter)
        {
            return await PagedListCreator.CreateAsync(_context.Products.Where(filter), page, pageSize);
        }

        public async Task<PagedList<Product>> GetPagedListFilterAndOrderAsync<TOrderBy>(int page, int pageSize, Expression<Func<Product, bool>> filter, Expression<Func<Product, TOrderBy>> selector)
        {
            return await PagedListCreator.CreateAsync(_context.Products.Where(filter).OrderBy(selector), page, pageSize);
        }

        public async Task<PagedList<Product>> GetPagedListFilterAndOrderDescAsync<TOrderBy>(int page, int pageSize, Expression<Func<Product, bool>> filter, Expression<Func<Product, TOrderBy>> selector)
        {
            return await PagedListCreator.CreateAsync(_context.Products.Where(filter).OrderByDescending(selector), page, pageSize);
        }

        public async Task<PagedList<Product>> GetPagedListOrderAsync<TOrderBy>(int page, int pageSize, Expression<Func<Product, TOrderBy>> selector)
        {
            return await PagedListCreator.CreateAsync(_context.Products.OrderBy(selector), page, pageSize);
        }

        public async Task<PagedList<Product>> GetPagedListOrderDescAsync<TOrderBy>(int page, int pageSize, Expression<Func<Product, TOrderBy>> selector)
        {
            return await PagedListCreator.CreateAsync(_context.Products.OrderByDescending(selector), page, pageSize);
        }

        //quries for favourite products

        public async Task<PagedList<Product>> GetPagedListFavouriteAsync(int page, int pageSize, Guid userId)
        {
            return await PagedListCreator.CreateAsync(_context.Products.Where(p => _context.FavouriteProducts.
            Where(fp => fp.UserId == userId).
            Select(fp => fp.ProductId).Contains(p.Id)),
            page, pageSize);
        }

        public async Task<PagedList<Product>> GetPagedListFilterFavouriteAsync(int page, int pageSize, Expression<Func<Product, bool>> filter, Guid userId)
        {
            return await PagedListCreator.CreateAsync(_context.Products.Where(filter).Where(p => _context.FavouriteProducts.
            Where(fp => fp.UserId == userId).
            Select(fp => fp.ProductId).Contains(p.Id)), page, pageSize);
        }

        public async Task<PagedList<Product>> GetPagedListFilterAndOrderFavouriteAsync<TOrderBy>(int page, int pageSize, Expression<Func<Product, bool>> filter, Expression<Func<Product, TOrderBy>> selector, Guid userId)
        {
            return await PagedListCreator.CreateAsync(_context.Products.Where(filter).Where(p => _context.FavouriteProducts.
            Where(fp => fp.UserId == userId).
            Select(fp => fp.ProductId).Contains(p.Id)).OrderBy(selector), page, pageSize);
        }

        public async Task<PagedList<Product>> GetPagedListFilterAndOrderDescFavouriteAsync<TOrderBy>(int page, int pageSize, Expression<Func<Product, bool>> filter, Expression<Func<Product, TOrderBy>> selector, Guid userId)
        {
            return await PagedListCreator.CreateAsync(_context.Products.Where(filter).Where(p => _context.FavouriteProducts.
            Where(fp => fp.UserId == userId).
            Select(fp => fp.ProductId).Contains(p.Id)).OrderByDescending(selector), page, pageSize);
        }

        public async Task<IEnumerable<string>> GetAllCategoriesAsync()
        {
            return await _context.Products.
                Select(p => p.Category).
                Distinct().
                ToListAsync();
        }
    }
}
