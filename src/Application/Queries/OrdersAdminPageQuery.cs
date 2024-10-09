using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Queries
{
    public class OrdersAdminPageQuery
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string[]? OrderStatuses { get; set; } = null;
        public Guid? UserId { get; set; } = null;
        public Guid? OrderId { get; set; } = null;
    }
}
