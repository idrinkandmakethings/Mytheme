using ElectronNET.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Mytheme
{
    public class Program
    {
        public static void Main(string[] args)
        {

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.File(@"logs\mytheme.log", rollingInterval: RollingInterval.Day)
                        .CreateLogger();

                    Log.Information($"===================================================================");
                    Log.Information($"Mytheme version {Constants.APP_VERSION}");
                    Log.Information("Copyright 2019");
                    Log.Information($"===================================================================");
                    webBuilder.UseElectron(args).UseStartup<Startup>();
                   // webBuilder.UseStartup<Startup>();

                   Log.CloseAndFlush();
                });
    }
}
