using Account.Management.Infrastructure.IdentitySeeder;
using Account.Management.Web;

namespace Account.Management.Infrastructure.Extentions
{
    public static class StartupExtensions
    {
        public static async Task SeedInitialDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                await RoleSeeder.SeedRolesAsync(services);

                await UserSeeder.SeedAdminUserAsync(services, "user.admin@gmail.com", "admin123", "Admin");
                await UserSeeder.SeedAdminUserAsync(services, "user.accountant@gmail.com", "accountant123", "Accountant");
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Seeding failed.");
            }
        }
    }
}
