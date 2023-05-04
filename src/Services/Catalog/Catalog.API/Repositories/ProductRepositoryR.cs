﻿using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Entities.Dtos;
using Catalog.API.Extensions;
using Catalog.API.Filter;
using Catalog.API.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using static Catalog.API.Entities.Dtos.CEnums;

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
        public async Task<(long totalRecords, IEnumerable<Category>)> GetProductsByCategory(PaginationFilter pagefilter, FilterDto myfilter)
        {
            SortDefinition<Category> sortDefinition = myfilter.IsAccending
                ? Builders<Category>.Sort.Ascending(myfilter.Orderby.ToString())
                : Builders<Category>.Sort.Descending(myfilter.Orderby.ToString());

            return await _context.CategoryList
                .AggregateByPage(Builders<Category>.Filter.Eq(x => x.CategoryName, myfilter.FilterItemName),
                sortDefinition,
                page: pagefilter.PageNumber,
                pageSize: pagefilter.PageSize);
        }
        public async Task<(long totalRecords, IEnumerable<Category>)> GetProductsBySubCategory(PaginationFilter pagefilter, FilterDto myfilter)
        {
            SortDefinition<Category> sortDefinition = myfilter.IsAccending
                 ? Builders<Category>.Sort.Ascending(myfilter.Orderbyvalue)
                 : Builders<Category>.Sort.Descending(myfilter.Orderbyvalue);

            return await _context.CategoryList
                .AggregateByPage(Builders<Category>.Filter.Eq(x => x.SubCategory.SubCategoryName, myfilter.FilterItemName),
                sortDefinition,
                page: pagefilter.PageNumber,
                pageSize: pagefilter.PageSize);
        }
        public async Task<(long totalRecords, IEnumerable<Category>)> GetProductsByName(PaginationFilter pagefilter, FilterDto myfilter)
        {

            // Create a sort definition based on the field name and sort direction
            SortDefinition<Category> sortDefinition = myfilter.IsAccending
                ? Builders<Category>.Sort.Ascending(myfilter.Orderbyvalue)
                : Builders<Category>.Sort.Descending(myfilter.Orderbyvalue);

            return await _context.CategoryList
                .AggregateByPage(Builders<Category>.Filter.Eq(x => x.SubCategory.Product.Name, myfilter.FilterItemName),
                sortDefinition,
                page: pagefilter.PageNumber,
                pageSize: pagefilter.PageSize);
        }
        public async Task<IEnumerable<Category>> GetProductsByMFP(string mfp, string mf)
        {
            var filter1 = Builders<Category>.Filter.Eq(x => x.SubCategory.Product.ManufacturerPartNo, mfp);
            var filter2 = Builders<Category>.Filter.Eq(x => x.SubCategory.Product.Manufacturer, mf);
            var res = await _context.CategoryList.Find(filter1 & filter2).ToListAsync();
            return res;
        }


    }
}
