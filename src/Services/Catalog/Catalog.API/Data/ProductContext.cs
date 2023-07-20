using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class ProductContext : IProductContext
    {
        public ProductContext(DbContextSettings dbContextSettings)
        {
            var client = new MongoClient(dbContextSettings.ConnectionString);
            var database = client.GetDatabase(dbContextSettings.DatabaseName);

            CategoryList = database.GetCollection<Category>(dbContextSettings.CollectionName);
            ProductContextSeed.SeedData(CategoryList);
        }
        public IMongoCollection<Category> CategoryList { get; }
    }
}
