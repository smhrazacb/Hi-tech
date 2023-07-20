using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepositoryW : IProductRepositoryW
    {
        private readonly IProductContext _context;

        public ProductRepositoryW(IProductContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
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
            return  updateResult.ModifiedCount > 0;
        }
        public async Task<BulkWriteResult<Category>> UpdateProducts(IEnumerable<Category> products)
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
            return await _context.CategoryList.BulkWriteAsync(bulkOps);
        }
    }
}
