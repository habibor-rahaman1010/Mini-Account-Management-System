using Account.Management.Infrastructure.DbContexts;
using Account.Management.Infrastructure.Extentions;
using Account.Management.Infrastructure.IdentitySeeder;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Reflection;

namespace Account.Management.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region Bootstrap Logger
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connection = configuration.GetConnectionString("DefaultConnection");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug().WriteTo.MSSqlServer(
                    connectionString: connection,
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "ApplicationLogs", AutoCreateSqlTable = false })
                .ReadFrom.Configuration(configuration)
                .CreateBootstrapLogger();
            #endregion

            try
            {
                Log.Information("Application Starting...");
                var builder = WebApplication.CreateBuilder(args);

                #region Serilog Configuration
                builder.Host.UseSerilog((hostBuilderContext, loggerConfiguration) =>
                {
                    loggerConfiguration.MinimumLevel.Debug()
                    .WriteTo.MSSqlServer(
                        connectionString: connection,
                        sinkOptions: new MSSqlServerSinkOptions { TableName = "ApplicationLogs", AutoCreateSqlTable = false })
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .ReadFrom.Configuration(builder.Configuration);

                });
                #endregion             

                // Add services to the container.
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                var migrationAssembly = Assembly.GetExecutingAssembly() ?? throw new InvalidOperationException("Migration Assembly not found.");

                builder.Services.RegisterServices(connectionString, migrationAssembly);

                builder.Services.AddDatabaseDeveloperPageExceptionFilter();

                builder.Services.AddIdentity();

                //This service for automapper
                builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

                builder.Services.AddControllersWithViews();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseMigrationsEndPoint();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseRouting();

                app.UseAuthorization();

                app.MapStaticAssets();

                app.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}")
                    .WithStaticAssets();

                await app.SeedInitialDataAsync();

                await app.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application Crashed...");
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }
    }
}
