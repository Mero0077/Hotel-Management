
using Hotel_Management.Data;
using Hotel_Management.DTOs.Reservation;
using Hotel_Management.Repositories;
using Hotel_Management.Services;
using HotelReservationSystem.api.Services.FacilitiesService;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Scalar.AspNetCore;
using Hotel_Management.DTOs.Offers;
using Hotel_Management.DTOs.Account;
using Hotel_Management.Services.IServices;



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

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationDbContext>();



            var key = Encoding.ASCII.GetBytes(Constants.SecretKey);
            builder.Services.AddAuthentication(opt=>opt.DefaultAuthenticateScheme= JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>{
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                        ValidIssuer = Constants.Issuer,
                        ValidAudience = Constants.Audience,

                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                      


                    };

            });

            builder.Services.AddAuthorization();

            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("All", policy => policy.RequireRole("Student,Instructor"));
            });

            builder.Services.AddScoped(typeof(GeneralRepository<>));
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<RoleFeatureService>();
          

            builder.Services.AddScoped<ReservationService>();
            builder.Services.AddScoped<FacilityService>();
            builder.Services.AddScoped<RoomTypeService>();
            builder.Services.AddScoped<RoomService>();

            builder.Services.AddScoped<ReservationService>();
            builder.Services.AddScoped<OfferService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddMemoryCache();
            

            builder.Services.AddAutoMapper(
                typeof(ReservationProfile).Assembly
                ,typeof(OfferProfile).Assembly,
                typeof(AccountProfile).Assembly
                );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapScalarApiReference();
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
