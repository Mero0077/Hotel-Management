using System.ComponentModel.DataAnnotations;

namespace Hotel_Management.Models.ViewModels.Reports
{
    public class GetBookingReportVM
    {
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
    }
}
