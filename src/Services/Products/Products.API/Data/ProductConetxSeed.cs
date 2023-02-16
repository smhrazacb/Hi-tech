using MongoDB.Driver;
using Products.API.Entities;

namespace Products.API.Data
{
    public class ProductConetxSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();
            if (!existProduct)
            {
                productCollection.InsertManyAsync(GetPreconfiguredProducts());
            }
        }
        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                     Id = "602d2149e773f2a3990b47f5",
                     Name = "Diode"
                } };
        }
    }
}
