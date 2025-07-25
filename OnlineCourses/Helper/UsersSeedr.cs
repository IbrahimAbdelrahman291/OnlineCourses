using DAL.Context;
using DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace OnlineCourses.Helper
{
    public static class UsersSeedr
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            // This method is intentionally left empty.
            // It can be used to seed initial user data if needed in the future.
            if (!userManager.Users.Any())
            {
                var user = new AppUser() 
                {
                    DisplayName = "Admin",
                    UserName = "AB",
                    Email = "AB@gmail.com",
                };
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await userManager.CreateAsync(user, "P@$$w0rdAB");
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
