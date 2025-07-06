using AutoMapper;
using Hotel_Management.DTOs.Reports;
using Hotel_Management.Filters;
using Hotel_Management.Models.Enums;
using Hotel_Management.Models.ViewModels.Errors;
using Hotel_Management.Models.ViewModels.Reports;
using Hotel_Management.Services;
using Hotel_Management.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;
        private readonly IMapper _mapper;
        private readonly IReportPdfService _bookingReportPdfService;

        public ReportController(ReportService reportService,IMapper mapper,IReportPdfService bookingReportPdfService) 
        {
            this._reportService = reportService;
            this._mapper = mapper;
            this._bookingReportPdfService = bookingReportPdfService;
        }

        [HttpPost]
        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.BookingReport})]
        public async Task<ResponseVM<IEnumerable<BookingReportVM>>> BookingReport([FromBody]GetBookingReportVM getBookingReportVM)
        {
            var mapped = _mapper.Map<GetBokkingReportRequestDTO>(getBookingReportVM);

            var response = await _reportService.BookingReportAsync(mapped);

            var responseMapped = _mapper.Map<IEnumerable<BookingReportVM>>(response);

            var pdf = _bookingReportPdfService.GenerateBookingReportPdf(responseMapped,getBookingReportVM.From,getBookingReportVM.To);
            return new SuccessResponseVM<IEnumerable<BookingReportVM>>(responseMapped);
        }
        [HttpGet]
        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.GenerateBookingReportPDF })]
        public async Task<IActionResult> GenerateBookingReportPDF([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var dto = new GetBokkingReportRequestDTO()
            {
                From = from,
                To = to
            };

            var response = await _reportService.BookingReportAsync(dto);

            var viewModel = _mapper.Map<IEnumerable<BookingReportVM>>(response);

            var pdfBytes = _bookingReportPdfService.GenerateBookingReportPdf(viewModel, from, to);

            return File(pdfBytes, "application/pdf", "booking-report.pdf");
        }


        [HttpGet]
        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.GenerateRevenueReportPDF })]
        public async Task<IActionResult> GenerateRevenueReportPDF([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var result = await _reportService.GenerateRevenueReportPDFAsync(from, to);
            var viewModel = _mapper.Map<IEnumerable<RevenueReportVM>>(result);

            var pdf = _bookingReportPdfService.GenerateRevenueReportPdf(viewModel, from, to);


            return File(pdf, "application/pdf", "revenue-report.pdf");

        }

        [HttpGet]
        [Authorize]
        [TypeFilter<CustomAuthorizeFilter>(Arguments = new object[] { Features.GenerateCustomerReportPDF })]
        public async Task<IActionResult> GenerateCustomerReportPDF([FromQuery] DateTime from, [FromQuery,] DateTime to)
        {
            var result = await _reportService.GenerateCustomerReportPDFAsync(from, to);
            var viewModel = _mapper.Map<IEnumerable<CustomerReportVM>>(result);
            var pdf = _bookingReportPdfService.GenerateCustomerReportPdf(viewModel, from, to);
            return File(pdf, "application/pdf", "customer-report.pdf");

        }

    }
}
