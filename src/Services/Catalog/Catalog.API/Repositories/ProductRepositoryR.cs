using MongoDB.Bson;
using MongoDB.Driver;
using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using System.Xml.Linq;
using DnsClient.Protocol;
using System;
using Catalog.API.Filter;
using System.Drawing.Printing;

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
        public async Task<(int totalpages, IEnumerable<Category>)> GetProductsByCategory(PaginationFilter pagefilter, string _categoryName)
        {
            var countFacet = AggregateFacet.Create("count",
           PipelineDefinition<Category, AggregateCountResult>.Create(new[]
           {
                PipelineStageDefinitionBuilder.Count<Category>()
           }));

            var dataFacet = AggregateFacet.Create("data",
                PipelineDefinition<Category, Category>.Create(new[]
                {
                PipelineStageDefinitionBuilder.Sort(Builders<Category>.Sort.Ascending(x => x.CategoryName)),
                PipelineStageDefinitionBuilder.Skip<Category>((pagefilter.PageNumber- 1) * pagefilter.PageSize),
                PipelineStageDefinitionBuilder.Limit<Category>(pagefilter.PageSize),
                }));

            var filter = Builders<Category>.Filter.Empty;
            var aggregation = await _context.CategoryList.Aggregate()
                .Match(filter)
                .Facet(countFacet, dataFacet)
                .ToListAsync();

            var count = aggregation.First()
                .Facets.First(x => x.Name == "count")
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
            ?.Count ?? 0;

            var totalPages = (int)count / pagefilter.PageSize;

            var data = aggregation.First()
                .Facets.First(x => x.Name == "data")
                .Output<Category>();

            return (totalPages, data);
            //return await _context
            //    .CategoryList
            //    .Find(p => p.CategoryName == _categoryName)
            //    .ToListAsync();
        }
        public async Task<IEnumerable<Category>> GetProductsBySubCategory(string _subCategoryName)
        {
            return await _context
                .CategoryList
                .Find(a => a.SubCategory.SubCategoryName == _subCategoryName)
                .ToListAsync();
        }
        public async Task<IEnumerable<Category>> GetProductsByName(string _name)
        {
            return await _context
                         .CategoryList
                         .Find(p => p.SubCategory.Product.Name.ToLower().Contains(_name.ToLower()))
                         .ToListAsync();
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
