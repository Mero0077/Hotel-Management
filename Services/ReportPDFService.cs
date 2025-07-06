using Hotel_Management.Models.ViewModels.Reports;
using Hotel_Management.Services.IServices;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hotel_Management.Services
{
    public class ReportPDFService : IReportPdfService
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

        public byte[] GenerateCustomerReportPdf(IEnumerable<CustomerReportVM> data, DateTime from, DateTime to)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    // Header
                    page.Header()
                        .Text("Customer Report")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    // Content
                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Text($"📅 From: {from:yyyy-MM-dd}   To: {to:yyyy-MM-dd}").FontSize(12);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1); // #
                                columns.RelativeColumn(4); // Email
                                columns.RelativeColumn(2); // Bookings
                                columns.RelativeColumn(3); // Total Spent
                            });

                            // Table Header
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("#");
                                header.Cell().Element(CellStyle).Text("Email");
                                header.Cell().Element(CellStyle).Text("Total Bookings");
                                header.Cell().Element(CellStyle).Text("Total Spent (EGP)");

                                static IContainer CellStyle(IContainer container) =>
                                    container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).Background(Colors.Grey.Lighten3).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                            });

                            // Table Rows
                            int index = 1;
                            foreach (var item in data)
                            {
                                table.Cell().Element(RowStyle).Text(index++.ToString());
                                table.Cell().Element(RowStyle).Text(item.Email);
                                table.Cell().Element(RowStyle).Text(item.TotalBookings.ToString());
                                table.Cell().Element(RowStyle).Text($"{item.TotalSpent:N2}");

                                static IContainer RowStyle(IContainer container) =>
                                    container.PaddingVertical(2).BorderBottom(1).BorderColor(Colors.Grey.Lighten3);
                            }
                        });

                        // Summary
                        var totalCustomers = data.Count();
                        var totalSpent = data.Sum(x => x.TotalSpent);

                        col.Item().PaddingTop(15).Text($"Total Customers: {totalCustomers}");
                        col.Item().Text($"Total Revenue: {totalSpent:N2} EGP");
                    });

                    // Footer
                    page.Footer()
                        .AlignCenter()
                        .Text(txt =>
                        {
                            txt.Span("Generated at: ").SemiBold();
                            txt.Span($"{DateTime.Now:yyyy-MM-dd HH:mm}");
                        });
                });
            })
               .GeneratePdf();
        }

        public byte[] GenerateRevenueReportPdf(IEnumerable<RevenueReportVM> revenueReportVMs, DateTime from, DateTime to)
        {
            var totalBookings = revenueReportVMs.Sum(x => x.BookingCount);
            var totalRevenue = revenueReportVMs.Sum(x => x.TotalRevenue);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Text("💵 Revenue Report").Bold().FontSize(18).FontColor(Colors.Green.Medium);

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"🗓️ From: {from:yyyy-MM-dd} - To: {to:yyyy-MM-dd}").FontSize(13).Bold();
                        col.Item().PaddingVertical(10);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2); // Date
                                columns.RelativeColumn(2); // Bookings
                                columns.RelativeColumn(2); // Revenue
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Date");
                                header.Cell().Element(CellStyle).Text("Bookings");
                                header.Cell().Element(CellStyle).Text("Revenue");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold()).Padding(5).Background(Colors.Grey.Lighten3).BorderBottom(1);
                                }
                            });

                            foreach (var row in revenueReportVMs)
                            {
                                table.Cell().Padding(5).Text(row.Date.ToString("yyyy-MM-dd"));
                                table.Cell().Padding(5).Text(row.BookingCount.ToString());
                                table.Cell().Padding(5).Text($"{row.TotalRevenue:C}");
                            }
                        });

                        col.Item().PaddingTop(15);
                        col.Item().Text($"🔢 Total Bookings: {totalBookings}").Bold();
                        col.Item().Text($"💰 Total Revenue: {totalRevenue:C}").Bold();
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

