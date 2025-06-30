using System.ComponentModel.DataAnnotations;

namespace Hotel_Management.Models.ViewModels.Account
{
    public class SendEmailVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
