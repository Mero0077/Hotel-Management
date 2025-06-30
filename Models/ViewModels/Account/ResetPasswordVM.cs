using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hotel_Management.Models.ViewModels.Account
{
    public class ResetPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string OTP { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
        [Required]
        [PasswordPropertyText]
        [Compare(nameof(Password),ErrorMessage ="Password Doesnot Match")]
        public string ConfirmedPassword { get; set; }
    }
}
