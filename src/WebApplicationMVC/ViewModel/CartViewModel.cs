using Store.Application.DataTransferObjects;

namespace Store.WebApplicationMVC.ViewModel
{
    public class CartViewModel
    {
        public decimal TotalPrice { get; set; }
        public IEnumerable<ProductDetailsDto> ProductDetails { get; set; }
        public bool IsUserLoggedIn { get; set; }
    }
}
