
using Hotel_Management.Data;
using Hotel_Management.DTOs.Reservation;
using Hotel_Management.Repositories;
using Hotel_Management.Services;
<<<<<<< HEAD
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
=======
using Scalar.AspNetCore;
>>>>>>> 52cdda6aad8d4bebf01e90a6eceeda0042cc86d0

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
<<<<<<< HEAD

            var key = Encoding.ASCII.GetBytes(Constants.SecretKey);
            builder.Services.AddAuthentication(opt=>opt.DefaultAuthenticateScheme=  JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>{
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                        ValidAudience = builder.Configuration["Front_Audience"],

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
          

            builder.Services.AddScoped< ReservationService>();
=======
            builder.Services.AddScoped<ReservationService>();
            builder.Services.AddScoped<OfferService>();
>>>>>>> 52cdda6aad8d4bebf01e90a6eceeda0042cc86d0

            builder.Services.AddAutoMapper(typeof(ReservationProfile).Assembly);

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
