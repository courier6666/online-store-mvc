using Store.Domain.Entities;

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
