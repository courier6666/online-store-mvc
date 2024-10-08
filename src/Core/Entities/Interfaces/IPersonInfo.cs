﻿namespace Store.Domain.Entities.Interfaces
{
    public interface IPersonInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly Birthday { get; set; }

    }
}
