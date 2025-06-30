using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hotel_Management.Models.ViewModels.Account
{
    public class RegisterVM
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [MinLength(11)]
        public string PhoneNumber { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
        [Required]
        [PasswordPropertyText]
        [Compare(nameof(Password),ErrorMessage ="Password doesnot match")]
        public string ConfirmedPassword { get; set; }
    }
}
