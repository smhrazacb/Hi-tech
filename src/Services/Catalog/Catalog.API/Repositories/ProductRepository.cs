using MongoDB.Bson;
using MongoDB.Driver;
using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using System.Xml.Linq;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IProductContext _context;

        public ProductRepository(IProductContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Category>> GetProducts()
        {
            return await _context
                                .CategoryList
                                .Find(p => true)
                                .ToListAsync();
        }
        public async Task<IEnumerable<Category>> GetProductsById(string id)
        {
            return null;
                //await _context
                //.CategoryList
                //.Find(p => p.Id == id)
                //.ToListAsync();
        }
        public async Task<IEnumerable<Category>> GetProductsByCategory(string category)
        {
            return await _context
                .CategoryList
                .Find(p => p.CategoryName == category)
                .ToListAsync();
        }
        public async Task<IEnumerable<Category>> GetProductsBySubCategory(string subCategory)
        {
            //return await _context
            //    .CategoryList
            //    .Find(p => p.SubCategories.Where(a=>a.SubCategoryName == subCategory).ToList())
            //    .ToListAsync();
            return null;
        }
        public async Task CreateProduct(Category product)
        {
            await _context.CategoryList.InsertOneAsync(product);
        }
        public async Task DeleteProduct(string id)
        {
            await _context.CategoryList.DeleteOneAsync(id);
        }
        public async Task<bool> UpdateProduct(Category product)
        {
            //var updateResult = await _context
            //                            .CategoryList
            //                            .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            //return updateResult.IsAcknowledged
            //                   && updateResult.ModifiedCount > 0;
            return false;
        }

        public async Task<IEnumerable<Category>> GetProductsByName(string name)
        {
            return null;
            //return await _context
            //             .CategoryList
            //             .Find(p => p.SubCategoryName == name)
            //             .ToListAsync();
        }

        public async Task<IEnumerable<CategoryWithCount>> GetCategoryList()
        {
            var pp = _context
                           .CategoryList
                           .Aggregate()
                           .Group(
                x => new { x.SubCategory,},
                g => new CategoryWithCount
                {
                    SubCategory = g.Key.SubCategory.SubCategoryName,
                    Count = g.Count(),
                }).ToListAsync();
            return null;
        }

        public Task<IEnumerable<Category>> GetSubCategoryList()
        {
            throw new NotImplementedException();
        }
    }
}
