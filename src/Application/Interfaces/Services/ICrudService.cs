using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Interfaces.Services
{
    public interface ICrudService<TModel> where TModel : class
    {
        Task<IEnumerable<TModel>> GetAllAsync();

        Task<TModel> GetByIdAsync(Guid id);

        Task<Guid> AddAsync(TModel model);

        Task UpdateAsync(TModel model);

        Task DeleteAsync(Guid modelId);
    }
}
