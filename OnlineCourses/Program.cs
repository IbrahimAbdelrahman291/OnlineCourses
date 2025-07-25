
using BAL.GenericRepository;
using BAL.Interfaces;
using DAL.Context;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineCourses.Helper;
using System.Text;

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
            builder.Services.AddScoped<ITokenServices, TokenServices>();
            builder.Services.AddIdentity<AppUser, IdentityRole>(options => 
            {
                options.Password.RequireDigit = true;             
                options.Password.RequiredLength = 6;              
                options.Password.RequireNonAlphanumeric = true;   
                options.Password.RequireUppercase = true;         
                options.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<CoursesDbContext>().AddDefaultTokenProviders();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(Options => 
            {
                Options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Validaud"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
                };
            });
            
            var app = builder.Build();

            using var Scpoe = app.Services.CreateScope();
            var services = Scpoe.ServiceProvider;
            var UserManager = services.GetRequiredService<UserManager<AppUser>>();
            var RoleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await UsersSeedr.SeedUsersAsync(UserManager,RoleManager);
            


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
