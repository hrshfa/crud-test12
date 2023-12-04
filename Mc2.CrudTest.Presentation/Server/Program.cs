using Mc2.CrudTest.Presentation.Front;
using Mc2.CrudTest.Presentation.Server.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;

namespace Mc2.CrudTest.Presentation.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var builder= CreateHostBuilder(args).Build();

            // load serilog.json to IConfiguration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                // reloadOnChange will allow you to auto reload the minimum level and level switches
                .AddJsonFile(path: "serilog.json", optional: false, reloadOnChange: true)
                .Build();

            // build Serilog logger from IConfiguration
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            Log.Logger = logger;



            var scopeFactory = builder.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializerService>();
            dbInitializer.Initialize();
            dbInitializer.SeedData();

            builder.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
    }
}
