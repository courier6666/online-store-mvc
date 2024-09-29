using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Queries
{
    public class ProductsPageQuery
    {
        public int PageSize { get; set; }
        public int Page {  get; set; }
        public string? Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortOrder { get; set; }
        public string? ProductName { get; set; }
    }
}
