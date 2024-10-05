namespace Store.Application.DataTransferObjects
{
    public class UserRegistrationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly Birthday { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public AddressDto Address { get; set; }
    }
}
