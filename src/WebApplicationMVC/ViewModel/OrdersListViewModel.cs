using Store.Application.DataTransferObjects;
using Store.WebApplicationMVC.Models;

namespace Store.WebApplicationMVC.ViewModel
{
    public class OrdersListViewModel
    {
        public IEnumerable<OrderDto> Orders { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string? OrderStatus { get; set; }
    }
}
