using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.DataTransferObjects
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public Guid OrderAuthorId { get; set; }
        public ICollection<ProductDetailsForOrderDto> ProductDetails { get; set; }
    }
}
