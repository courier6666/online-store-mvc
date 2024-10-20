using Store.Domain.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Repositories
{
    public interface IFavouriteProductsRepository : IRepository<FavouriteProduct, (Guid userId, Guid productId)>
    {
        Task<Guid[]> AreTheseFavoruiteProductsOfUserAsync(Guid userId, Guid[] productIds);
    }
}
