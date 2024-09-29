using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities
{
    /// <summary>
    /// Used for adding products and their quantity into order.
    /// </summary>
    public class OrderProductDetail : Entity<Guid>
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        /// <summary>
        /// Quantity of product added to order.
        /// </summary>
        public uint ItemsCount { get; set; }
        /// <summary>
        /// Total price of a product. 
        /// </summary>
        public decimal TotalPrice
        {
            get
            {
                return Product.Price * ItemsCount;
            }
        }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
