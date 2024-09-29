using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Store.Domain.PagedLists
{
    /// <summary>
    /// Paged list data structure used for loading data in chunks.
    /// </summary>
    /// <typeparam name="T">Type of data stored in list.</typeparam>
    public class PagedList<T> : IEnumerable<T>
    {
        public static PagedList<T> Empty()
        {
            return new PagedList<T>(new List<T>(), 0, 0, 0);
        }
        public PagedList(IList<T> items, int count, int pageIndex, int pageSize)
        {
            Items = items;
            TotalCount = count;
            Page = pageIndex;
            PageSize = pageSize;
        }
        /// <summary>
        /// Data stored in list.
        /// </summary>
        public IList<T> Items { get; }
        /// <summary>
        /// Current page of data chunk.
        /// </summary>
        public int Page { get; }
        /// <summary>
        /// Size of page.
        /// </summary>
        public int PageSize { get; }
        /// <summary>
        /// Total count of all elements in general
        /// </summary>
        public int TotalCount { get; }
        /// <summary>
        /// Returns true if there is a next page.
        /// </summary>
        public bool HasNextPage => Page * PageSize < TotalCount;
        /// <summary>
        /// Returns true if there is a previous page.
        /// </summary>
        public bool HasPreviousPage => Page > 1;

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}
