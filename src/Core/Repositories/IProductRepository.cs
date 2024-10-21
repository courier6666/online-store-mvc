using Store.Domain.Entities;
using Store.Domain.PagedLists;
using System.Linq.Expressions;

namespace Store.Domain.Repositories
{
    public interface IProductRepository : IPagedListRepository<Product, Guid>
    {
        Task<IEnumerable<Product>> GetByFilterAsync(Expression<Func<Product, bool>> filter);
        Task<IEnumerable<string>> GetAllCategoriesAsync();
        Task<PagedList<Product>> GetPagedListFavouriteAsync(int page, int pageSize, Guid userId);
        Task<PagedList<Product>> GetPagedListFilterFavouriteAsync(int page, int pageSize, Expression<Func<Product, bool>> filter, Guid userId);
        Task<PagedList<Product>> GetPagedListFilterAndOrderFavouriteAsync<TOrderBy>(int page, int pageSize, Expression<Func<Product, bool>> filter, Expression<Func<Product, TOrderBy>> selector, Guid userId);
        Task<PagedList<Product>> GetPagedListFilterAndOrderDescFavouriteAsync<TOrderBy>(int page, int pageSize, Expression<Func<Product, bool>> filter, Expression<Func<Product, TOrderBy>> selector, Guid userId);
    }
}
