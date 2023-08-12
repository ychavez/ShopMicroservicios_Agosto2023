using Catalog.Api.Data;
using Catalog.Api.Entities;

namespace Catalog.Api.Repositories
{
    public class ProductRepository
    {
        private readonly ICatalogContext catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            this.catalogContext = catalogContext;
        }

        public async Task CreateProduct(Product product)
            => await catalogContext.Products.InsertOneAsync(product);
    }
}
