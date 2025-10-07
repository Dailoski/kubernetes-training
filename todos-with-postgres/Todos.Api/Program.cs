
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using DbUp;
using System.Reflection;
using System.Diagnostics;
using Todos.Api.Extensions;

namespace Todos.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDatabase<Program>()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddEnvironmentVariables();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}