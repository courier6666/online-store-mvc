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
        Task<(bool isFavourite, Guid productId)[]> AreFavouriteProductsAsync(Guid userId, Guid[] productIds);
        Task RemoveFavouriteProductForUserAsync(Guid userId, Guid productId);
        Task AddFavouriteProductForUserAsync(Guid userId, Guid productId);
    }
}
