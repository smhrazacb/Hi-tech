using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MongoDB.Driver.Core.Configuration;
using Moq;
using Npgsql;
using Ordering.API.Services;
using Ordering.Infrastructure.Persistence;
using ServicesTest.Infrastructure;
using System.Data.Common;
using TestData;
using Webhooks.API;
using Webhooks.API.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ServicesTest.Services
{
    public class WebhookWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public string ConnectionString { get; set; }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var context = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(WebhooksContext));

                services.Remove(context);

                // Create open SqliteConnection so EF won't automatically close it.

                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                services.AddDbContext<WebhooksContext>(options =>
               options.UseSqlite(connection, sqliteOptionsAction: sqlOptions =>
               {
                   sqlOptions.MigrationsAssembly(typeof(Startup).Assembly.FullName);
               }));
                services.Replace(ServiceDescriptor.Scoped(_ =>
                {
                    var _IIdentityService = new Mock<IIdentityService>();
                    _IIdentityService.Setup(Object => Object.GetUserIdentity()).Returns(BasketData.GetBasketData().UserId);
                    return _IIdentityService.Object;
                }));
            });
            builder.UseEnvironment("Development");
        }
    }
}