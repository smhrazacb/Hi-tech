using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories.Interfaces
{
    public interface IProductRepositoryW
    {
        Task<bool> UpdateProduct(Category product);
        Task UploadProducts(IEnumerable<Category> products);
        Task<BulkWriteResult<Category>> UpdateProducts(IEnumerable<Category> products);
        Task<bool> DeleteProduct(string id);
    }
}
