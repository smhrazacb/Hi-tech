using Products.API.Entities;
using Products.API.Repositories.Interfaces;

namespace Products.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public Task CreateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProducts()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProductsByCategory(string category)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProductsByID(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetProductsBySubCategory(string subCategory)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
