using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CurrencyRateGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"BUILD_CONFIGURATION - Конфигурация сборки: {Environment.GetEnvironmentVariable("BUILD_CONFIGURATION")}");
            Console.WriteLine($"ASPNETCORE_ENVIRONMENT - Среда выполнения: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostingContext, loggerConfig) => 
                {
                    loggerConfig.ReadFrom.Configuration(hostingContext.Configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
