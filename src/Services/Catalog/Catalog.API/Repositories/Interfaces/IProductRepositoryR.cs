using Catalog.API.Entities;
using Catalog.API.Filter;
using MongoDB.Driver;

namespace Catalog.API.Repositories.Interfaces
{
    public interface IProductRepositoryR
    {
        Task<IEnumerable<CategoryWithCount>> GetProducts();
        Task<Category> GetProductById(string id);
        Task<(long totalRecords, IEnumerable<Category>)> GetProductsByCategory(PaginationFilter pagefilter, string category);
        Task<(long totalRecords, IEnumerable<Category>)> GetProductsBySubCategory(PaginationFilter pagefilter, string category);
        Task<(long totalRecords, IEnumerable<Category>)> GetProductsByName(PaginationFilter pagefilter, string name);
        Task<IEnumerable<Category>> GetProductsByMFP(string mfp, string mf);
    }
}
