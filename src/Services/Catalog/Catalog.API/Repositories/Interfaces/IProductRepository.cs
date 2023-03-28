using Catalog.API.Entities;

namespace Catalog.API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<CategoryWithCount>> GetProducts();
        Task<Category> GetProductById(string id);
        Task<IEnumerable<Category>> GetProductsByCategory(string category);
        Task<IEnumerable<Category>> GetProductsBySubCategory(string subCategory);
        Task<IEnumerable<Category>> GetProductsByName(string name);
        Task<IEnumerable<Category>> GetProductsByMFP(string mfp, string mf);
        Task CreateProduct(Category product);
        Task UploadProducts(IEnumerable<Category> products);
        Task<bool> UpdateProduct(Category product);
        Task UpdateProducts(IEnumerable<Category> products);
        Task<bool> DeleteProduct(string id);
    }
}
