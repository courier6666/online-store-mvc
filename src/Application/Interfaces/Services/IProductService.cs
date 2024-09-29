using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Store.Application.DataTransferObjects;
using Store.Application.Queries;
using Store.Domain.PagedLists;

namespace Store.Application.Interfaces.Services
{
    /// <summary>
    /// Provides services for managing products including creating, retrieving,
    /// and updating products, as well as getting paged product lists.
    /// </summary>
    public interface IProductService : ICrudService<ProductDto>
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<PagedList<ProductDto>> GetPagedProductsAsync(int page, int pageSize);
        Task<PagedList<ProductDto>> GetPagedProductsAsync(ProductsPageQuery productsPageQuery);
        Task<IEnumerable<string>> GetAllCategoriesAsync();
    }
}
