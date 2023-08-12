using Catalog.Api.Entities;

namespace Catalog.Api.Repositories
{
    public interface IProductRepository
    {
        Task CreateProduct(Product product);
        Task<IEnumerable<Product>> GetProducts();
    }
}