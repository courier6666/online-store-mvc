﻿using Store.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Model
{
    /// <summary>
    /// Class that represents address of user.
    /// </summary>
    public class Address : Entity<Guid>
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public Guid PersonId { get; set; }
        public IUser Person { get; set; }
    }
}
