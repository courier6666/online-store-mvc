using Store.Domain.Entities.Model;

namespace Store.Domain.Entities.Interfaces
{
    public interface IUser : IEntity<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly Birthday { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public Address Address { get; set; }
        public ICollection<CashDeposit> CashDeposits { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
