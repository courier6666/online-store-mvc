using Newtonsoft.Json;
using Store.Application.DataTransferObjects;
using Store.Application.Interfaces.Services;
using Store.WebApplicationMVC.Extensions;

namespace Store.WebApplicationMVC.Services
{
    public class SessionCartService : ICartService
    {
        [JsonIgnore]
        public ISession? Session { get; set; }

        public static ICartService GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
            SessionCartService cart = session?.GetJson<SessionCartService>("Cart") ?? new SessionCartService();
            cart.Session = session;
            return cart;
        }
        private List<ProductDetailsDto> _lines = new List<ProductDetailsDto>();
        public IReadOnlyList<ProductDetailsDto> Lines => _lines;

        public void AddItem(ProductDto product, int quantity)
        {
            var cartLine = _lines.
            Where(l => l.Product.Id == product.Id).
            FirstOrDefault();

            if (cartLine == null)
            {
                _lines.Add(new()
                {
                    Product = product,
                    ItemsCount = (uint)quantity
                });
            }
            else
            {
                cartLine.ItemsCount += (uint)quantity;
            }

            this.Session?.SetJson("Cart", this);
        }

        public void Clear()
        {
            _lines.Clear();
            this.Session?.SetJson("Cart", this);
        }

        public decimal ComputeTotalValue()
        {
            return _lines.Sum(l => l.ItemsCount * l.Product.Price);
        }

        public void RemoveItem(ProductDto product)
        {
            _lines.RemoveAll(l => l.Product.Id == product.Id);
            this.Session?.Remove("Cart");
        }
    }
}
