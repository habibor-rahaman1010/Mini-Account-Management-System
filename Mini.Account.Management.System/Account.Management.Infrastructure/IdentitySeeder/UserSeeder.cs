using Account.Management.Infrastructure.Account.Management.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Account.Management.Infrastructure.IdentitySeeder
{
    public static class UserSeeder
    {
        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider, string email, string password, string role)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("UserSeeder");

            try
            {
                var existingUser = await userManager.FindByEmailAsync(email);

                if (existingUser == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(adminUser, password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, role);
                        logger.LogInformation("Admin user created and assigned to Admin role.");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            logger.LogError($"Error creating admin user: {error.Description}");
                        }
                    }
                }
                else
                {
                    logger.LogInformation("Admin user already exists.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the admin user.");
            }
        }
    }
}
