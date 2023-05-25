using Catalog.API.Data;
using Catalog.API.Entities;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesTest.Services
{
    public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureAuth(IWebHostBuilder app)
        {

        }

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