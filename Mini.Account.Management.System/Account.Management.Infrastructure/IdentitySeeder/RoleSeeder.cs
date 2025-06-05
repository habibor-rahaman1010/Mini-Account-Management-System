using Account.Management.Infrastructure.Account.Management.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Management.Infrastructure.IdentitySeeder
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            string[] roles = new string[] { "Admin", "Accountant", "Viewer" };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole()
                    {
                        Id = Guid.NewGuid(),
                        Name = role,
                        NormalizedName = role.ToUpper(),
                        ConcurrencyStamp = DateTime.UtcNow.Ticks.ToString()
                    });
                }
            }
        }
    }
}
