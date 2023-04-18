using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data.Interfaces
{
    public interface IProductContext
    {
        IMongoCollection<Category> CategoryList { get; }
    }
}
