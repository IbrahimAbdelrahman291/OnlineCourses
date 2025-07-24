using DAL.Context;
using DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace OnlineCourses.Helper
{
    public static class UsersSeedr
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            // This method is intentionally left empty.
            // It can be used to seed initial user data if needed in the future.
            if (!userManager.Users.Any())
            {
                var user = new AppUser() 
                {
                    DisplayName = "Admin",
                    UserName = "admin",
                    Email = "admin@gmail.com",
                };
                await userManager.CreateAsync(user, "P@$$w0rd");
            }
        }
    }
}
