using Store.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Model
{
    public class FavouriteProduct
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public virtual IUser User { get; set; } = null;
        public virtual Product Product { get; set; } = null;
    }
}
