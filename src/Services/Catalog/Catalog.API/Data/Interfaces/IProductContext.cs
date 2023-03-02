using MongoDB.Driver;
using Catalog.API.Entities;

namespace Catalog.API.Data.Interfaces
{
    public interface IProductContext
    {
       IMongoCollection<Category> CategoryList { get; }
    }
}
