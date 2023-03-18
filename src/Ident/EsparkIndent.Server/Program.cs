using EsparkIndent.Server.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace EsparkIndent.Server;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args)
            .Build()
            .MigrateDatabase<ApplicationDbContext>((context, services) =>
            {
                var logger = services.GetService<ILogger<AppicationContextSeed>>();
                AppicationContextSeed
                    .SeedAsync(services, logger)
                    .Wait();
            })
            .Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>());
}
