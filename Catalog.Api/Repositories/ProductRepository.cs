using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            this.catalogContext = catalogContext;
        }

        public async Task CreateProduct(Product product)
            => await catalogContext.Products.InsertOneAsync(product);

        public async Task<IEnumerable<Product>> GetProducts()
              => await catalogContext.Products.Find(x => true).ToListAsync();

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Id, id);

            DeleteResult deleteResult = await catalogContext.Products.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await catalogContext.Products.ReplaceOneAsync(filter: x => x.Id == product.Id, product);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<Product> GetProduct(string id)
        {
            return await catalogContext.Products.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
