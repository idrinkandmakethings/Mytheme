using System;
using System.IO;
using ElectronNET.API;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Mytheme
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Mytheme");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(Path.Combine(basePath, @"logs\mytheme.log"), rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information($"===================================================================");
            Log.Information($"Mytheme version {Constants.APP_VERSION}");
            Log.Information("Copyright 2019");
            Log.Information($"===================================================================");


            BuildWebHost(args).Run();
            //CreateHostBuilder(args).Build().Run();

            Log.CloseAndFlush();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
               // .UseElectron(args)
                .Build();
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            Log.Logger = new LoggerConfiguration()
        //                .MinimumLevel.Debug()
        //                .WriteTo.File(@"logs\mytheme.log", rollingInterval: RollingInterval.Day)
        //                .CreateLogger();

        //            Log.Information($"===================================================================");
        //            Log.Information($"Mytheme version {Constants.APP_VERSION}");
        //            Log.Information("Copyright 2019");
        //            Log.Information($"===================================================================");

        //            webBuilder.UseElectron(args).UseStartup<Startup>();
        //            // webBuilder.UseStartup<Startup>();

        //            Log.CloseAndFlush();
        //        });
    }
}
