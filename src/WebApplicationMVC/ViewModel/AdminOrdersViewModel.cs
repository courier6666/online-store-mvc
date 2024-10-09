using Store.Application.DataTransferObjects;
using Store.WebApplicationMVC.Models;

namespace Store.WebApplicationMVC.ViewModel
{
    public class AdminOrdersViewModel
    {
        public IEnumerable<OrderDto> Orders { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public OrderStatusViewModel[] OrderStatuses { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? UserId { get; set; }
    }
}
