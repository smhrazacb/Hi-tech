using Catalog.API.Entities;

namespace Catalog.API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<CategoryWithCount>> GetProducts();
        Task<IEnumerable<Category>> GetProductsById(string id);
        Task<IEnumerable<Category>> GetProductsByCategory(string category);
        Task<IEnumerable<Category>> GetProductsBySubCategory(string subCategory);
        Task<IEnumerable<Category>> GetProductsByName(string name);
        Task CreateProduct(Category product);
        Task<bool> UpdateProduct(Category product);
        Task<bool> DeleteProduct(string id);
    }
}
