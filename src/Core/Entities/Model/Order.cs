using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Domain.Entities.Interfaces;
using Store.Domain.Entities.Model;

namespace Store.Domain.Entities
{
    /// <summary>
    /// Represents order that is created by user.
    /// </summary>
    public class Order : Entity<Guid>
    {
        /// <summary>
        /// Order's constructor that initializes all collection.
        /// Collection with entries of order is SortedSet. It is used to store entries in ascending order by creation date.
        /// </summary>
        public Order()
        {
            ProductDetails = new Collection<OrderProductDetail>();
            Entries = new SortedSet<Entry>(Comparer<Entry>.Create((a, b) => a.CreatedDate.CompareTo(b.CreatedDate)));
        }
        public bool IsOrderPayed { get; set; }
        public OrderStatus Status { get; set; }
        public Guid OrderAuthorId { get; set; }
        public IUser OrderAuthor { get; set; }
        public ICollection<OrderProductDetail> ProductDetails { get; set; }
        /// <summary>
        /// Property that calculates total price of order. 
        /// </summary>

        public decimal TotalPrice
        {
            get
            {
                return Math.Round(ProductDetails.Sum(pd => pd.TotalPrice), 2);
            }
        }
        /// <summary>
        /// Collection that stores order history in ascending order by creation date.
        /// </summary>
        public ICollection<Entry> Entries { get; set; }
        public virtual PaymentDetails PaymentDetails { get; set; }
    }
}
