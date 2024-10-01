using Store.WebApplicationMVC.Validation;
using System.ComponentModel.DataAnnotations;

namespace Store.WebApplicationMVC.ViewModel
{
    public class RegistrationViewModel
    {
        public string ReturnUrl { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Required, Age(18)]
        public DateTime? Birthday { get; set; } = null;
        [Required]
        public string Login { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string Country { get; set; }
        public string State { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
    }
}
