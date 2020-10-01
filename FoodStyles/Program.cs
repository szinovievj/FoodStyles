using System.IO;
using FoodStyles.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace FoodStyles
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();
            
            CreateHostBuilder(args).Build()
                .MigrateDatabase<ApplicationDbContext>()
                .Run();
            
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseSerilog() 
                        .UseStartup<Startup>();
                });
    }
}