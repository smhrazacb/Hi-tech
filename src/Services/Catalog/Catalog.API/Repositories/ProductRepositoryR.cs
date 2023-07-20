using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Entities.Dtos;
using Catalog.API.Extensions;
using Catalog.API.Filter;
using Catalog.API.Repositories.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Catalog.API.Repositories
{
    public class ProductRepositoryR : IProductRepositoryR
    {
        private readonly IProductContext _context;
        public ProductRepositoryR(IProductContext context)
        {
            _context = context;
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
        public async Task<FilterResult> GetFilteredProducts
            (PaginationFilter pagefilter, FilterSortDto myfilter)
        {
            SortDefinition<Category> sortDefinition = (bool)myfilter.Sortdto.IsAccending
                              ? Builders<Category>.Sort.Ascending(myfilter.Sortdto.Orderbyvalue())
                              : Builders<Category>.Sort.Descending(myfilter.Sortdto.Orderbyvalue());

            var filters = Builders<Category>.Filter.Empty;
            if (myfilter.Filters != null || myfilter.Filters.Count() != 0)
            {
                var filtersdef = new List<FilterDefinition<Category>>();
                foreach (var item in myfilter.Filters)
                {
                    if (item.FilterValue != null)
                        filtersdef.Add(Builders<Category>.Filter.Eq(item.Filetrbyvalue(), item.FilterValue));
                }
                if (filtersdef.Count != 0)
                    filters = Builders<Category>.Filter.And(filtersdef);
            }
            var (a, b) = await _context.CategoryList
                .AggregateByPage(filters, sortDefinition, page: pagefilter.PageNumber, pageSize: pagefilter.PageSize);

            var additionalFieldsgroup = await GetAdditionalData(filters);

            return new FilterResult() { TotalRecords = a, Items = b, AdditionalFilters = additionalFieldsgroup };
        }

        private async Task<Dictionary<string, int>> GetAdditionalData(FilterDefinition<Category> filters)
        {
            // exclude id, return only gender and date of birth
            var additionalFieldsProjection = Builders<Category>.Projection
                .Exclude(e => e.Id)
                .Include(u => u.SubCategory.Product.AdditionalFields);

            // Get results
            var additionalFields = await _context.CategoryList
                .Find(filters)
                .Project(additionalFieldsProjection) // projection stage
                .As<Category>()
                .ToListAsync();

            var additionalFieldsgroup = additionalFields
                .SelectMany(a => a.SubCategory.Product.AdditionalFields)
                .ToList()
                .GroupBy(ii => ii.Key)
                .ToDictionary(group => group.Key, group => group.Count());
            return additionalFieldsgroup;
        }

        public async Task<IEnumerable<Category>> GetProductsByMFP(string mfp, string mf)
        {
            var filter1 = Builders<Category>.Filter.Eq(x => x.SubCategory.Product.ManufacturerPartNo, mfp);
            var filter2 = Builders<Category>.Filter.Eq(x => x.SubCategory.Product.Manufacturer, mf);
            var res = await _context.CategoryList.Find(filter1 & filter2).ToListAsync();
            return res;
        }
    }
    public class UnwindData
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public Dictionary<string, string> AdditionalFields { get; set; }
    }
}
