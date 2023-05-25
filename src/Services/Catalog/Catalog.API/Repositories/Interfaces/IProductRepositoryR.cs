using Catalog.API.Entities;
using Catalog.API.Entities.Dtos;
using Catalog.API.Filter;

namespace Catalog.API.Repositories.Interfaces
{
    public interface IProductRepositoryR
    {
        Task<IEnumerable<CategoryWithCount>> GetProducts();
        Task<Category> GetProductById(string id);
        Task<FilterResult> GetFilteredProducts(PaginationFilter pagefilter, FilterSortDto myfilter);
        Task<IEnumerable<Category>> GetProductsByMFP(string mfp, string mf);
    }
}
