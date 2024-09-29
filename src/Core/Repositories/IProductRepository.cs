using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Store.Domain.Entities;
using Store.Domain.PagedLists;

namespace Store.Domain.Repositories
{
    public interface IProductRepository : IPagedListRepository<Product, Guid>
    {
        Task<IEnumerable<Product>> GetByFilterAsync(Expression<Func<Product, bool>> filter);
        Task<IEnumerable<string>> GetAllCategoriesAsync();
    }
}
