
using Hotel_Management.Data;
using Hotel_Management.DTOs.Reservation;
using Hotel_Management.Repositories;
using Hotel_Management.Services;
using HotelReservationSystem.api.Services.FacilitiesService;

namespace Hotel_Management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<ApplicationDbContext>();
            builder.Services.AddScoped(typeof(GeneralRepository<>));

            builder.Services.AddScoped< ReservationService>();
            builder.Services.AddScoped<FacilityService>();
            builder.Services.AddScoped<RoomTypeService>();
            builder.Services.AddScoped<RoomService>();

            builder.Services.AddAutoMapper(typeof(ReservationProfile).Assembly);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
