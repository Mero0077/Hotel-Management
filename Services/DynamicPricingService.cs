using AutoMapper;
using Hotel_Management.Models;
using Hotel_Management.Models.Enums;
using Hotel_Management.Repositories;
using Microsoft.AspNetCore.DataProtection;

namespace Hotel_Management.Services
{
    public class DynamicPricingService
    {
        private readonly GeneralRepository<Room> _roomRepository;

        public DynamicPricingService(GeneralRepository<Room> roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public decimal GetDynamicPricing(DateTime startDate, DateTime endDate, decimal basePrice)
        {
            decimal price = basePrice;

            var allRoomsCount = _roomRepository.GetAll().Count();
            var occupiedRoomsCount = _roomRepository.Get(e => e.Status == RoomStatus.Occupied).Count();

            if (occupiedRoomsCount / (decimal)allRoomsCount > 0.8m)
            {
                price *= 1.20m; // CORRECT: just multiply by factor
            }

            if ((startDate - DateTime.Now).TotalDays <= 2)
            {
                price *= 1.15m;
            }

            if (startDate.Month >= 6 && startDate.Month <= 8)
            {
                price *= 1.10m;
            }

            if (startDate.DayOfWeek == DayOfWeek.Friday || startDate.DayOfWeek == DayOfWeek.Saturday)
            {
                price *= 1.05m;
            }

            return price;
        }

    }

}
