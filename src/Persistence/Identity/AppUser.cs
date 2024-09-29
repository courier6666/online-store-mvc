using Microsoft.AspNetCore.Identity;
using Store.Domain.Entities;
using Store.Domain.Entities.Interfaces;
using Store.Domain.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.Main.Identity
{
    public class AppUser : IdentityUser<Guid>, IUser
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly Birthday { get; set; }
        public string Login { get => UserName; set => UserName = value; }
        public Address Address { get; set; }
        public ICollection<CashDeposit> CashDeposits { get; set; }
        public ICollection<Order> Orders { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public byte[] Version { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
