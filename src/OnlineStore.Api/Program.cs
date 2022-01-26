using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Polly;
using Polly.Retry;
using System;
using System.Threading.Tasks;
using OnlineStore.Api.Domain.Orders;
using OnlineStore.Api.Infrastructure.EntityFramework;
using OnlineStore.Api.Infrastructure.EntityFramework.Data;
using OnlineStore.Api.Infrastructure.Azure.Interfaces;

namespace OnlineStore.Api
{
    public class Program
    {
        private static readonly RetryPolicy _retryPolicy = Policy.Handle<Exception>().WaitAndRetry(5, _ => TimeSpan.FromSeconds(5));

        public static async Task Main(string[] args)
        {
            Console.WriteLine($"Current environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                _retryPolicy.Execute(() => context.Database.Migrate());

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                _retryPolicy.Execute(() => UserData.Seed(userManager));

                var blobStorage = scope.ServiceProvider.GetRequiredService<IBlobStorage>();
                await blobStorage.SetupContainersAsync();

                var queueStorage = scope.ServiceProvider.GetRequiredService<IQueueStorage>();
                await queueStorage.SetupQueueAsync("orders");
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureKestrel(options => options.AddServerHeader = false)
                        .UseStartup<Startup>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddApplicationInsights();
                    logging.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Trace);
                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
                });
    }
}
