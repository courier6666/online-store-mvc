using Store.Application.DataTransferObjects;
using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
