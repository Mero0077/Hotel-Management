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

        public  decimal GetDynamicPricing(DateTime startDate, DateTime endDate, decimal basePrice)
        {
            decimal price = basePrice;

            var rooms = _roomRepository.GetAll();

            var allRoomsCount = rooms.Count();
            var occupiedRoomsCount = rooms.Count(r => r.Status == RoomStatus.Occupied);


            if (occupiedRoomsCount / (decimal)allRoomsCount > (decimal)0.8)
            {
                price *= (decimal)1.20;
            }

            if ((startDate - DateTime.Now).TotalDays <= 2)
            {
                price *= (decimal)1.15;
            }

            if (startDate.Month >= 6 && startDate.Month <= 8)
            {
                price *= (decimal)1.10;
            }

            if (startDate.DayOfWeek == DayOfWeek.Friday || startDate.DayOfWeek == DayOfWeek.Saturday)
            {
                price *= (decimal)1.05;
            }

            return price;
        }
    }

}
