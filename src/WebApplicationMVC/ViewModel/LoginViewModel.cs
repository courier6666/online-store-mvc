using System.ComponentModel.DataAnnotations;

namespace Store.WebApplicationMVC.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Name must not be empty!")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Password must not be empty!")]
        public string? Password { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}
