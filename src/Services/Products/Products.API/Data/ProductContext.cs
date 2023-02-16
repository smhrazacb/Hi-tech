using MongoDB.Driver;
using Products.API.Data.Interfaces;
using Products.API.Entities;

namespace Products.API.Data
{
    public class ProductContext : IProductContext
    {
        public ProductContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            ProductList = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));

        }
        public IMongoCollection<Product> ProductList { get; }

    }
}
