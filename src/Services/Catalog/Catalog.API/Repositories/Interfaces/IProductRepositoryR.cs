using Catalog.API.Entities;
using Catalog.API.Entities.Dtos;
using Catalog.API.Filter;
using static Catalog.API.Entities.Dtos.CEnums;

namespace Catalog.API.Repositories.Interfaces
{
    public interface IProductRepositoryR
    {
        Task<IEnumerable<CategoryWithCount>> GetProducts();
        Task<Category> GetProductById(string id);
        Task<(long totalRecords, IEnumerable<Category>)> GetProductsByCategory(PaginationFilter pagefilter, SortFilterDto myfilter);
        Task<(long totalRecords, IEnumerable<Category>)> GetProductsBySubCategory(PaginationFilter pagefilter, SortFilterDto myfilter);
        Task<(long totalRecords, IEnumerable<Category>)> GetProductsByName(PaginationFilter pagefilter, SortFilterDto myfilter);
        Task<IEnumerable<Category>> GetProductsByMFP(string mfp, string mf);
    }
}
