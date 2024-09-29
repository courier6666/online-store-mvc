using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.DataTransferObjects
{
    public class EntryDto
    {
        public Guid? Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Message { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public override string ToString()
        {
            return $"{Message} At {CreatedDate.Date.ToString("f")}. Order id - '{OrderId}'";
        }
    }
}
