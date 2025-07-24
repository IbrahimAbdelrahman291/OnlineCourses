
using BAL.GenericRepository;
using BAL.Interfaces;
using DAL.Context;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineCourses.Helper;

namespace OnlineCourses
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));// any thing use IGenericRepository will use GenericRepository
            // Add DbContext with SQL Server configuration
            builder.Services.AddDbContext<CoursesDbContext>(options => 
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // Add Identity services
            builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<CoursesDbContext>().AddDefaultTokenProviders();
            builder.Services.AddAuthentication();
            
            var app = builder.Build();

            using var Scpoe = app.Services.CreateScope();
            var services = Scpoe.ServiceProvider;
            var UserManager = services.GetRequiredService<UserManager<AppUser>>();
            await UsersSeedr.SeedUsersAsync(UserManager);
            


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
