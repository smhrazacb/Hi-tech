using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Npgsql;
using Ordering.Infrastructure.Persistence;
using System.Data.Common;
using TestData;
using Webhooks.API;
using Webhooks.API.Data;
using Webhooks.API.Services;

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

                services.Replace(ServiceDescriptor.Scoped(_ =>
                {
                    var _IIdentityService = new Mock<IIdentityService>();
                    _IIdentityService.Setup(Object => Object.GetUserIdentity()).Returns(BasketData.GetBasketData().UserId);
                    return _IIdentityService.Object;
                }));

                var scope = services.BuildServiceProvider().GetService<IConfiguration>();
                ConnectionString = scope.GetSection("ConnectionStrings:WebhookConnectionString").Value;
                //delete old database
                DropDatabase();
                services.AddDbContext<WebhooksContext>(options =>
                options.UseNpgsql(ConnectionString));

                services.Replace(ServiceDescriptor.Scoped(_ =>
                {
                    var _IIdentityService = new Mock<IIdentityService>();
                    _IIdentityService.Setup(Object => Object.GetUserIdentity()).Returns(BasketData.GetBasketData().UserId);
                    return _IIdentityService.Object;
                }));
            });
            builder.UseEnvironment("Development");
        }
        public void DropDatabase()
        {
            DbConnectionStringBuilder builder = new();
            builder.ConnectionString = ConnectionString;
            string database = builder["Database"] as string;
            using (var connection = new NpgsqlConnection("Server=localhost;Port=6432;Database=orderdb;User Id=admin;Password=admin1234;"))
            {

                connection.Open();

                // Ensure that you have the necessary permissions to drop the database.

                // Create the SQL command to drop the database.
                var sqlCommand = $"DROP DATABASE IF EXISTS \"{database}\" WITH (FORCE)";

                using (var command = new NpgsqlCommand(sqlCommand, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}