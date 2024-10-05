using Store.Application.DataTransferObjects;

namespace Store.Application.Interfaces.Services
{
    public interface ICartService
    {
        public IReadOnlyList<ProductDetailsDto> Lines { get; }
        public void AddItem(ProductDto product, int quantity);
        public void RemoveItem(ProductDto product);
        public decimal ComputeTotalValue();
        public void Clear();
    }
}
