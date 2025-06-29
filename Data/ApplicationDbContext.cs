using Hotel_Management.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Hotel_Management.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): DbContext(options)
    {
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
<<<<<<< HEAD
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
=======
        public DbSet<RoleFeature> RoleFeatures { get; set; }    
        public DbSet<User> Users { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
>>>>>>> 04ba319229d4a3dbebc5a2c7de5c49bb78611931

        public DbSet<RoomOffer> RoomOffers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
<<<<<<< HEAD
            //optionsBuilder.UseSqlServer("Server=.;Database=HotelDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True")
            optionsBuilder
=======
            optionsBuilder.UseSqlServer("Server=.;Database=HotelManagement;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True")
>>>>>>> 04ba319229d4a3dbebc5a2c7de5c49bb78611931
                 .LogTo(log => Debug.WriteLine(log), LogLevel.Information).EnableSensitiveDataLogging(true).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}
