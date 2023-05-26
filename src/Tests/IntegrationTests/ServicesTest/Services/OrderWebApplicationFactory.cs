using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver.Core.Configuration;
using Npgsql;
using Ordering.Infrastructure.Persistence;
using System.Data.Common;

namespace ServicesTest.Services
{
    public class OrderWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public string ConnectionString { get; set; }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            { 
              
                var oderContext = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(OrderContext));

                services.Remove(oderContext);

                var scope = services.BuildServiceProvider().GetService<IConfiguration>();
                ConnectionString = scope.GetSection("ConnectionStrings:OrderingConnectionString").Value;
                //delete old database
                DropDatabase();
                services.AddDbContext<OrderContext>(options =>
                options.UseNpgsql(ConnectionString));

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