using Catalog.API.Entities;
using Catalog.API.Filter;
using MongoDB.Driver;

namespace Catalog.API.Repositories.Interfaces
{
    public interface IProductRepositoryR
    {
        Task<IEnumerable<CategoryWithCount>> GetProducts();
        Task<Category> GetProductById(string id);
        Task<(int totalpages, IEnumerable<Category>)> GetProductsByCategory(PaginationFilter pagefilter, string category);
        Task<IEnumerable<Category>> GetProductsBySubCategory(string subCategory);
        Task<IEnumerable<Category>> GetProductsByName(string name);
        Task<IEnumerable<Category>> GetProductsByMFP(string mfp, string mf);
    }
}
