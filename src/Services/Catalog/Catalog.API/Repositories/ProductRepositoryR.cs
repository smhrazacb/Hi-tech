using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Extensions;
using Catalog.API.Filter;
using Catalog.API.Repositories.Interfaces;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepositoryR : IProductRepositoryR
    {
        private readonly IProductContext _context;

        public ProductRepositoryR(IProductContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<CategoryWithCount>> GetProducts()
        {
            return await _context
                                 .CategoryList
                                 .Aggregate()
                                 .Group(
                 a => new { a.CategoryName, a.SubCategory.SubCategoryName },
                 b => new CategoryWithCount
                 {
                     CategoryName = b.Key.CategoryName,
                     SubCategoryName = b.Key.SubCategoryName,
                     SubCategoryCount = b.Count()
                 })
                                 .ToListAsync();
        }
        public async Task<Category> GetProductById(string _id)
        {
            return await _context
                .CategoryList
                .Find(p => p.Id == _id)
                .FirstOrDefaultAsync();
        }
        public async Task<(long totalRecords, IEnumerable<Category>)> GetProductsByCategory(PaginationFilter pagefilter, string _categoryName)
        {
            return await _context.CategoryList
                .AggregateByPage(Builders<Category>.Filter.Eq(x => x.CategoryName, _categoryName),
                Builders<Category>.Sort.Ascending(x => x.CategoryName),
                page: pagefilter.PageNumber,
                pageSize: pagefilter.PageSize);
        }
        public async Task<(long totalRecords, IEnumerable<Category>)> GetProductsBySubCategory(PaginationFilter pagefilter, string _subcatagoryname)
        {
            return await _context.CategoryList
                .AggregateByPage(Builders<Category>.Filter.Eq(x => x.SubCategory.SubCategoryName, _subcatagoryname),
                Builders<Category>.Sort.Ascending(x => x.CategoryName),
                page: pagefilter.PageNumber,
                pageSize: pagefilter.PageSize);
        }
        public async Task<(long totalRecords, IEnumerable<Category>)> GetProductsByName(PaginationFilter pagefilter, string _name)
        {
            return await _context.CategoryList
                .AggregateByPage(Builders<Category>.Filter.Eq(x => x.SubCategory.Product.Name, _name),
                Builders<Category>.Sort.Ascending(x => x.CategoryName),
                page: pagefilter.PageNumber,
                pageSize: pagefilter.PageSize);
        }
        public async Task<IEnumerable<Category>> GetProductsByMFP(string mfp, string mf)
        {
            var filter1 = Builders<Category>.Filter.Eq(x => x.SubCategory.Product.ManufacturerPartNo, mfp);
            var filter2 = Builders<Category>.Filter.Eq(x => x.SubCategory.Product.Manufacturer, mf);
            var res = await _context.CategoryList.Find(filter1 & filter2).ToListAsync();
            if (res == null)
            {
                Console.WriteLine();
            }
            return res;
        }
    }
}
