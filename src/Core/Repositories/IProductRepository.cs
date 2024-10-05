using Store.Domain.Entities;
using System.Linq.Expressions;

namespace Store.Domain.Repositories
{
    public interface IProductRepository : IPagedListRepository<Product, Guid>
    {
        Task<IEnumerable<Product>> GetByFilterAsync(Expression<Func<Product, bool>> filter);
        Task<IEnumerable<string>> GetAllCategoriesAsync();
    }
}
