using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities
{
    /// <summary>
    /// Represents a history record for order.
    /// used for logging order history.
    /// </summary>
    public class Entry : Entity<Guid>
    {
        /// <summary>
        /// Sets CreatedData toi current moment.
        /// </summary>
        public Entry()
        {
            CreatedDate = DateTime.Now;
        }
        public string Message { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
