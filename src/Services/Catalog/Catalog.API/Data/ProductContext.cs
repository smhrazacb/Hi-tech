using MongoDB.Driver;
using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;

namespace Catalog.API.Data
{
    public class ProductContext : IProductContext
    {
        public ProductContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            CategoryList = database.GetCollection<Category>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
            ProductContextSeed.SeedData(CategoryList);
        }
        public IMongoCollection<Category> CategoryList { get; }


    }
}
