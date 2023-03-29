﻿using MongoDB.Bson;
using MongoDB.Driver;
using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using System.Xml.Linq;
using DnsClient.Protocol;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IProductContext _context;

        public ProductRepository(IProductContext context)
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
        public async Task<IEnumerable<Category>> GetProductsByCategory(string _categoryName)
        {
            return await _context
                .CategoryList
                .Find(p => p.CategoryName == _categoryName)
                .ToListAsync();
        }
        public async Task<IEnumerable<Category>> GetProductsBySubCategory(string _subCategoryName)
        {
            return await _context
                .CategoryList
                .Find(a => a.SubCategory.SubCategoryName == _subCategoryName)
                .ToListAsync();
        }
        public async Task CreateProduct(Category _product)
        {
            await _context.CategoryList.InsertOneAsync(_product);
        }
        public async Task UploadProducts(IEnumerable<Category> _products)
        {
            await _context.CategoryList.InsertManyAsync(_products);
        }
        public async Task<bool> DeleteProduct(string _id)
        {
            var filter = Builders<Category>.Filter.Eq(p => p.Id, _id);
            DeleteResult deleteResult = await _context.CategoryList.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
        public async Task<bool> UpdateProduct(Category _product)
        {
            var updateResult = await _context
                                        .CategoryList
                                        .ReplaceOneAsync(filter: g => g.Id == _product.Id, replacement: _product);
            return updateResult.IsAcknowledged
                               && updateResult.ModifiedCount > 0;
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

            return
                await _context.CategoryList
                .Find(filter1 & filter2).ToListAsync();
        }

        public async Task UpdateProducts(IEnumerable<Category> products)
        {
            var bulkOps = new List<WriteModel<Category>>();
            foreach (var record in products)
            {
                var upsertOne = new ReplaceOneModel<Category>(
                    Builders<Category>.Filter.Where(x => x.Id == record.Id),
                    record)
                { IsUpsert = true };
                bulkOps.Add(upsertOne);
            }
            await _context.CategoryList.BulkWriteAsync(bulkOps);
        }
    }
}
