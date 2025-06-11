using Account.Management.Application.Services;
using Account.Management.Domain;
using Account.Management.Domain.RepositoriesInterface;
using Account.Management.Domain.ServicesInterface;
using Account.Management.Infrastructure.Account.Management.Identity;
using Account.Management.Infrastructure.DbContexts;
using Account.Management.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Account.Management.Infrastructure.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, string connectionString, Assembly migrationAssembly)
        {
            //Resolved here all service dependencies...
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString, (m) => m.MigrationsAssembly(migrationAssembly)));
            services.AddScoped<IApplicationTime, ApplicationTime>();
            services.AddScoped<IChartOfAccountRepository, ChartOfAccountRepository>();
            services.AddScoped<IVoucherTypeRepository, VoucherTypeRepository>();
            services.AddScoped<IVoucherTypeManagementService, VoucherTypeManagementService>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<IVoucherManagementService, VoucherManagementService>();
            services.AddScoped<IVoucherEntriesRepository, VoucherEntriesRepository>();
            services.AddScoped<IVoucherEntriesManagementService, VoucherEntriesManagementService>();

            return services;
        }


        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<ApplicationUserManager>()
                .AddRoleManager<ApplicationRoleManager>()
                .AddSignInManager<ApplicationSignInManager>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireRole("Admin");
                });

                options.AddPolicy("Accountant", policy =>
                {
                    policy.RequireRole("Accountant");
                });

                options.AddPolicy("Viewer", policy =>
                {
                    policy.RequireRole("Viewer");
                });
            });

            return services;
        }
    }
}
