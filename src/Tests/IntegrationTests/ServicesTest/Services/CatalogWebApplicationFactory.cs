using Catalog.API.Data;
using EventBus.Messages.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Reflection;

namespace ServicesTest.Services
{
    public class CatalogWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(ProductContext));

                services.Remove(dbContextDescriptor);

                var dDbContextSettings = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextSettings));

                services.Remove(dDbContextSettings);

                var scope = services.BuildServiceProvider().GetService<IConfiguration>();
                // Create open SqliteConnection so EF won't automatically close it.
                services.AddSingleton<ProductContext>();
                services.AddSingleton(p =>
                new DbContextSettings(
                    scope.GetSection("DatabaseSettings:ConnectionString").Value,
                     scope.GetSection("DatabaseSettings:DatabaseName").Value,
                     scope.GetSection("DatabaseSettings:CollectionName").Value
                    ));
            });
            builder.UseEnvironment("Development");
        }
    }
}