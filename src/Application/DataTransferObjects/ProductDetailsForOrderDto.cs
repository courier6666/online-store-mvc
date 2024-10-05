namespace Store.Application.DataTransferObjects
{
    public class ProductDetailsForOrderDto
    {
        public uint ItemsCount { get; set; }
        public ProductDto Product { get; set; }
    }
}
