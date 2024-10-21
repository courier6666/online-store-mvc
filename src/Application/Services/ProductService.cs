using Store.Application.DataTransferObjects;
using Store.Application.Interfaces.Mapper;
using Store.Application.Interfaces.Services;
using Store.Application.Queries;
using Store.Domain.Entities;
using Store.Domain.Entities.Interfaces;
using Store.Domain.Entities.Model;
using Store.Domain.PagedLists;
using System.Linq.Expressions;

namespace Store.Application.Services
{
    /// <summary>
    /// Provides services for managing products including creating, retrieving,
    /// and updating products, as well as getting paged product lists.
    /// </summary>
    public sealed class ProductService : IProductService
    {
        private readonly ICustomMapper _customMapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork, ICustomMapper customMapper)
        {
            _unitOfWork = unitOfWork;
            _customMapper = customMapper;
        }

        public async Task<Guid> AddAsync(ProductDto model)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var newProduct = _customMapper.Map<ProductDto, Product>(model);
                newProduct.Id = Guid.NewGuid();

                _unitOfWork.ProductRepository.Add(newProduct);

                await _unitOfWork.CommitAsync();
                return newProduct.Id;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException("Failed to create product!", innerException: e);
            }
        }

        public async Task DeleteAsync(Guid modelId)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _unitOfWork.ProductRepository.Delete(await _unitOfWork.ProductRepository.GetByIdAsync(modelId));
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            return _customMapper.MapEnumerable<Product, ProductDto>(await _unitOfWork.ProductRepository.GetAllAsync());
        }

        public async Task<IEnumerable<string>> GetAllCategoriesAsync()
        {
            return await _unitOfWork.ProductRepository.GetAllCategoriesAsync();
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns><typeparamref name="IEnumerable"/> of <typeparamref name="ProductDto"/></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var productsInfo = _customMapper.MapEnumerable<Product, ProductDto>(await _unitOfWork.ProductRepository.GetAllAsync());

                await _unitOfWork.CommitAsync();

                return productsInfo;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException("Failed to get all products!", innerException: e);
            }
        }

        public async Task<ProductDto> GetByIdAsync(Guid id)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
                var productInfo = _customMapper.Map<Product, ProductDto>(product);
                await _unitOfWork.CommitAsync();
                
                return productInfo;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException("Failed to get all products!", innerException: e);
            }
        }

        /// <summary>
        /// Retrieves a paged list of products.
        /// </summary>
        /// <param name="page">The page number.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>A paged list of product DTOs.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<PagedList<ProductDto>> GetPagedProductsAsync(int page, int pageSize)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var pagedProducts = await _unitOfWork.ProductRepository.GetPagedListAsync(page, pageSize);

                var pagedProductsMapped = _customMapper.MapPagedList<Product, ProductDto>(pagedProducts);

                await _unitOfWork.CommitAsync();

                return pagedProductsMapped;
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException("Failed to get paged products!", innerException: e);
            }
        }

        public async Task<PagedList<ProductDto>> GetPagedProductsAsync(ProductsPageQuery productsPageQuery)
        {
            try
            {
                _unitOfWork.BeginTransaction();


                Expression<Func<Product, bool>> exp = p =>
                (productsPageQuery.Category == null || p.Category == productsPageQuery.Category) &&
                (productsPageQuery.MinPrice == null || p.Price >= productsPageQuery.MinPrice) &&
                (productsPageQuery.MaxPrice == null || p.Price <= productsPageQuery.MaxPrice) &&
                (productsPageQuery.ProductName == null || p.Name.Contains(productsPageQuery.ProductName));

                PagedList<Product> pagedProducts = PagedList<Product>.Empty();

                switch (productsPageQuery.SortOrder)
                {
                    case "newest":
                        if (productsPageQuery.FavouriteProductsOfUser == null)
                        {
                            pagedProducts = await _unitOfWork.
                                ProductRepository.
                                GetPagedListFilterAndOrderDescAsync(productsPageQuery.Page, productsPageQuery.PageSize, exp, p => p.CreatedDate);
                        }
                        else
                        {
                            pagedProducts = await _unitOfWork.
                                ProductRepository.
                                GetPagedListFilterAndOrderDescFavouriteAsync(productsPageQuery.Page, productsPageQuery.PageSize, exp, p => p.CreatedDate, productsPageQuery.FavouriteProductsOfUser.Value);
                        }
                        break;
                    case "oldest":
                        if (productsPageQuery.FavouriteProductsOfUser == null)
                        {
                            pagedProducts = await _unitOfWork.
                                ProductRepository.
                                GetPagedListFilterAndOrderAsync(productsPageQuery.Page, productsPageQuery.PageSize, exp, p => p.CreatedDate);
                        }
                        else
                        {
                            pagedProducts = await _unitOfWork.
                                ProductRepository.
                                GetPagedListFilterAndOrderFavouriteAsync(productsPageQuery.Page, productsPageQuery.PageSize, exp, p => p.CreatedDate, productsPageQuery.FavouriteProductsOfUser.Value);
                        }
                        break;
                    case "cheapest":
                        if (productsPageQuery.FavouriteProductsOfUser == null)
                        {
                            pagedProducts = await _unitOfWork.
                               ProductRepository.
                               GetPagedListFilterAndOrderAsync(productsPageQuery.Page, productsPageQuery.PageSize, exp, p => p.Price);
                        }
                        else
                        {
                            pagedProducts = await _unitOfWork.
                                ProductRepository.
                                GetPagedListFilterAndOrderFavouriteAsync(productsPageQuery.Page, productsPageQuery.PageSize, exp, p => p.CreatedDate, productsPageQuery.FavouriteProductsOfUser.Value);
                        }
                        break;
                    case "priciest":
                        if (productsPageQuery.FavouriteProductsOfUser == null)
                        {
                            pagedProducts = await _unitOfWork.
                               ProductRepository.
                               GetPagedListFilterAndOrderDescAsync(productsPageQuery.Page, productsPageQuery.PageSize, exp, p => p.Price);
                        }
                        else
                        {
                            pagedProducts = await _unitOfWork.
                                ProductRepository.
                                GetPagedListFilterAndOrderDescFavouriteAsync(productsPageQuery.Page, productsPageQuery.PageSize, exp, p => p.CreatedDate, productsPageQuery.FavouriteProductsOfUser.Value);
                        }
                        break;
                    default:
                        if (productsPageQuery.FavouriteProductsOfUser == null)
                        {
                            pagedProducts = await _unitOfWork.
                               ProductRepository.
                               GetPagedListFilterAndOrderDescAsync(productsPageQuery.Page, productsPageQuery.PageSize, exp, p => p.Price);
                        }
                        else
                        {
                            pagedProducts = await _unitOfWork.
                                ProductRepository.
                                GetPagedListFilterAndOrderDescFavouriteAsync(productsPageQuery.Page, productsPageQuery.PageSize, exp, p => p.CreatedDate, productsPageQuery.FavouriteProductsOfUser.Value);
                        }
                        break;
                }

                await _unitOfWork.CommitAsync();

                return _customMapper.MapPagedList<Product, ProductDto>(pagedProducts);
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException("Failed to get paged products!", innerException: e);
            }
        }

        public async Task UpdateAsync(ProductDto model)
        {
            try
            {
                if (model.Id == null)
                    throw new InvalidOperationException("Cannot update product, id is not provided.");

                _unitOfWork.BeginTransaction();

                var foundProduct = await _unitOfWork.ProductRepository.GetByIdAsync(model.Id.Value);

                if (foundProduct == null)
                    throw new InvalidOperationException($"Cannot update product, product by id '{model.Id}' not found.");

                _customMapper.MapToExisting(model, ref foundProduct);

                _unitOfWork.ProductRepository.Update(foundProduct);

                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException("Failed to update product!", innerException: e);
            }
        }

        public async Task<(bool isFavourite, Guid productId)[]> AreFavouriteProductsAsync(Guid userId, Guid[] productIds)
        {
            var favouriteProducts = await _unitOfWork.FavoriteProductsRepository.AreTheseFavoruiteProductsOfUserAsync(userId, productIds);
            var favouriteProductsSet = new HashSet<Guid>(favouriteProducts);

            return productIds.
                Select(id => (favouriteProductsSet.Contains(id), id)).
                ToArray();
        }

        public async Task RemoveFavouriteProductForUserAsync(Guid userId, Guid productId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var foundFavProduct = await _unitOfWork.FavoriteProductsRepository.GetByIdAsync((userId, productId));
                _unitOfWork.FavoriteProductsRepository.Delete(foundFavProduct);

                await _unitOfWork.CommitAsync();
            }
            catch(Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }

        public async Task AddFavouriteProductForUserAsync(Guid userId, Guid productId)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                _unitOfWork.FavoriteProductsRepository.Add(new FavouriteProduct()
                {
                    ProductId = productId,
                    UserId = userId,
                });

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }
    }
}
