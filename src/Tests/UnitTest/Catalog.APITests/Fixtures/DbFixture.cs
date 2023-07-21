using Catalog.API.Data;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.APITests.Fixtures
{
    public class DbFixture : IDisposable
    {
        public DbFixture()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            DbContextSettings = new DbContextSettings(
                config.GetValue<string>("DatabaseSettings:ConnectionString"),
                config.GetValue<string>("DatabaseSettings:DatabaseName"),
                config.GetValue<string>("DatabaseSettings:CollectionName"),
                config.GetValue<string>("DatabaseSettings:ECollectionName")
                );
            dropDatabase();

            this.DbContext = new ProductContext(DbContextSettings);
        }
        public DbContextSettings DbContextSettings { get; }

        public ProductContext DbContext { get; }
        public void Dispose()
        {
            dropDatabase();
        }
        void dropDatabase()
        {
            var client = new MongoClient(DbContextSettings.ConnectionString);
            var databases = client.ListDatabaseNames().ToList();
            foreach (var item in databases)
            {
                if (item == DbContextSettings.DatabaseName)
                    client.DropDatabase(DbContextSettings.DatabaseName);
            }
        }
    }
}
