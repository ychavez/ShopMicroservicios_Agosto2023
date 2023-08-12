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
    }
}
