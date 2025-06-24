using Hotel_Management.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Hotel_Management.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }

        public DbSet<RoomOffer> RoomOffers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=HotelDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True")
                 .LogTo(log => Debug.WriteLine(log), LogLevel.Information).EnableSensitiveDataLogging(true).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}
