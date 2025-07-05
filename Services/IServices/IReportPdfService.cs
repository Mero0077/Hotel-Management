using Hotel_Management.Models.ViewModels.Reports;

namespace Hotel_Management.Services.IServices
{
    public interface IReportPdfService
    {
        byte[] GenerateBookingReportPdf(IEnumerable<BookingReportVM> bookingReportVMs,DateTime from,DateTime to);
        byte[] GenerateRevenueReportPdf(IEnumerable<RevenueReportVM> revenueReportVMs,DateTime from,DateTime to);
    }
}
