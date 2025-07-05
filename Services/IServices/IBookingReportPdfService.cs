using Hotel_Management.Models.ViewModels.Reports;

namespace Hotel_Management.Services.IServices
{
    public interface IBookingReportPdfService
    {
        byte[] GenerateBookingReportPdf(IEnumerable<BookingReportVM> bookingReportVMs,DateTime from,DateTime to);
    }
}
