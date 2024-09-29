using Store.Domain.Entities;
using Store.Domain.PagedLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Repositories
{
    public interface IPagedListRepository<TObject, TObjectId> : IRepository<TObject, TObjectId>
        where TObject : class
    {
        Task<PagedList<TObject>> GetPagedListAsync(int page, int pageSize);
        Task<PagedList<TObject>> GetPagedListFilterAsync(int page, int pageSize, Expression<Func<TObject, bool>> filter);
        Task<PagedList<TObject>> GetPagedListFilterAndOrderAsync<TOrderBy>(
            int page,
            int pageSize,
            Expression<Func<TObject, bool>> filter,
            Expression<Func<TObject, TOrderBy>> selector);

        Task<PagedList<TObject>> GetPagedListFilterAndOrderDescAsync<TOrderBy>(
            int page,
            int pageSize,
            Expression<Func<TObject, bool>> filter,
            Expression<Func<TObject, TOrderBy>> selector);
        Task<PagedList<TObject>> GetPagedListOrderAsync<TOrderBy>(
            int page,
            int pageSize,
            Expression<Func<TObject, TOrderBy>> selector);

        Task<PagedList<TObject>> GetPagedListOrderDescAsync<TOrderBy>(
            int page,
            int pageSize,
            Expression<Func<TObject, TOrderBy>> selector);
    }
}
