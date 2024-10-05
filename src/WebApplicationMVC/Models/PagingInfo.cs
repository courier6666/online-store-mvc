namespace Store.WebApplicationMVC.Models
{
    public class PagingInfo
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}
