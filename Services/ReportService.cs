
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using Hotel_Management.DTOs.Reports;
using Hotel_Management.Exceptions;
using Hotel_Management.Models;
using Hotel_Management.Models.Enums;
using Hotel_Management.Models.ViewModels.Reports;
using Hotel_Management.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Hotel_Management.Services
{
    public class ReportService
    {
        private readonly GeneralRepository<Reservation> _reservationRepository;
        private readonly IMapper _mapper;

        public ReportService(GeneralRepository<Reservation> reservationRepository,IMapper mapper) 
        {
            this._reservationRepository = reservationRepository;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<GetBookingReportResponseDTO>> BookingReportAsync(GetBokkingReportRequestDTO request)
        {
            ValidateDate(request.From,request.To);
            var query = _reservationRepository.Get(e => e.CheckInDate >= request.From && e.CheckOutDate <= request.To).ProjectTo<GetBookingReportResponseDTO>(_mapper.ConfigurationProvider);
            if (!await query.AnyAsync())
                throw new NotFoundException("No Booking Data Found", ErrorCode.ReportsNoData);
            return await query.ToListAsync();
        }

        private static void ValidateDate(DateTime from,DateTime to)
        {
            if (from == default && to == default)
                throw new ValidationException("Please provide valid From and To Dates", ErrorCode.InvalidDateFormat);
            if ((to - from).TotalDays > 31)
                throw new BusinessLogicException("Cannot Request More Than 31 Days", ErrorCode.DataRangeTooLarge);
            if (from >= to)
                throw new ValidationException("From date must be earlier than To date", ErrorCode.FromMustBeEarlierThanTo);
        }

        public async Task<IEnumerable<RevenueResponseDTO>> GenerateRevenueReportPDFAsync(DateTime from,DateTime to)
        {
            ValidateDate(from,to);
            var query = _reservationRepository.Get(e => e.CheckInDate >= from && e.CheckOutDate <= to);
            if (!await query.AnyAsync())
                throw new NotFoundException("No Data Found", ErrorCode.ReportsNoData);
            var queryGrouped = await query.GroupBy(e => e.CheckInDate.Date).Select(e => new RevenueResponseDTO()
            {
                BookingCount = e.Count(),
                Date = e.Key,
                TotalRevenue = e.Sum(r => r.TotalCost)
            }).OrderBy(e => e.Date).ToListAsync();
            return queryGrouped;
        }

    }
}
