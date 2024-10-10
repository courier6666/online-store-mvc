using Microsoft.AspNet.Identity;
using Store.Application.DataTransferObjects;

namespace Store.WebApplicationMVC.ViewModel
{
    public class AdminOrderDetailViewModel
    {
        public OrderDto Order { get; set; }
        public IUser OrderAuthor { get; set; }
    }
}
