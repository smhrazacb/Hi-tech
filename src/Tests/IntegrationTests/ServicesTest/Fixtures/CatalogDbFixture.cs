using Catalog.API.Data;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ServicesTest.Fixtures
{
    public class CatalogDbFixture : IDisposable
    {
        public CatalogDbFixture()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            DbContextSettings = new DbContextSettings(
                config.GetValue<string>("DatabaseSettings:ConnectionString"),
                $"test_db_{Guid.NewGuid()}",
                config.GetValue<string>("DatabaseSettings:CollectionName")
                );

            this.DbContext = new ProductContext(DbContextSettings);
        }
        public DbContextSettings DbContextSettings { get; }

        public ProductContext DbContext { get; }
        public void Dispose()
        {
            var client = new MongoClient(DbContextSettings.ConnectionString);
            client.DropDatabase(DbContextSettings.DatabaseName);
        }
    }
}
