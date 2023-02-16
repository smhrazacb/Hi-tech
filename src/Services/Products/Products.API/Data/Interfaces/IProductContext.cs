using MongoDB.Driver;
using Products.API.Entities;

namespace Products.API.Data.Interfaces
{
    public class IProductContext
    {
        IMongoCollection<Product> ProductList { get; }
    }
}
