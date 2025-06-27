using Hotel_Management.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Hotel_Management.Data
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoleFeature> RoleFeatures { get; set; }    
        public DbSet<User> Users { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }

        public DbSet<RoomOffer> RoomOffers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=HotelManagement;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True")
                 .LogTo(log => Debug.WriteLine(log), LogLevel.Information).EnableSensitiveDataLogging(true).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}
