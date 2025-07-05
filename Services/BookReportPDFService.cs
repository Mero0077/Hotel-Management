using Hotel_Management.Models.ViewModels.Reports;
using Hotel_Management.Services.IServices;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Hotel_Management.Services
{
    public class BookReportPDFService : IBookingReportPdfService
    {
        public byte[] GenerateBookingReportPdf(IEnumerable<BookingReportVM> bookingReportVMs, DateTime from, DateTime to)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Text("📋 Booking Report").Bold().FontSize(18).FontColor(Colors.Blue.Medium);

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"🗓️ From: {from:yyyy-MM-dd} - To: {to:yyyy-MM-dd}").FontSize(13).Bold();
                        col.Item().PaddingVertical(10);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2); // Customer Name
                                columns.RelativeColumn(1); // Room Number
                                columns.RelativeColumn(2); // Check-in
                                columns.RelativeColumn(2); // Check-out
                                columns.RelativeColumn(1); // Nights
                            });

                            // Header row
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Customer");
                                header.Cell().Element(CellStyle).Text("Room");
                                header.Cell().Element(CellStyle).Text("Check-In");
                                header.Cell().Element(CellStyle).Text("Check-Out");
                                header.Cell().Element(CellStyle).Text("Nights");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold()).Padding(5).Background(Colors.Grey.Lighten3).BorderBottom(1);
                                }
                            });

                            foreach (var booking in bookingReportVMs)
                            {
                                table.Cell().Padding(5).Text(booking.CustomerEmail);
                                table.Cell().Padding(5).Text(booking.RoomNumber);
                                table.Cell().Padding(5).Text(booking.CheckInDate.ToString("yyyy-MM-dd"));
                                table.Cell().Padding(5).Text(booking.CheckOutDate.ToString("yyyy-MM-dd"));
                                table.Cell().Padding(5).Text((booking.CheckOutDate - booking.CheckInDate).Days.ToString());
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(txt =>
                    {
                        txt.Span("Generated at ");
                        txt.Span($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}").SemiBold();
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
   }

