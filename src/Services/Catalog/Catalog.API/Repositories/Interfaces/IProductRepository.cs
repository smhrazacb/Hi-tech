using Catalog.API.Entities;

namespace Catalog.API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Category>> GetProducts();
        Task<IEnumerable<Category>> GetProductsById(string id);
        Task<IEnumerable<Category>> GetProductsByCategory(string category);
        Task<IEnumerable<Category>> GetProductsBySubCategory(string subCategory);
        Task<IEnumerable<Category>> GetProductsByName(string name);
        Task<IEnumerable<Category>> GetSubCategoryList();
        Task<IEnumerable<CategoryWithCount>> GetCategoryList();

        Task CreateProduct(Category product);
        Task<bool> UpdateProduct(Category product);
        Task DeleteProduct(string id);
    }
}
