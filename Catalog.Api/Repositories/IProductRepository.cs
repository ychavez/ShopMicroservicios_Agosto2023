using Catalog.Api.Entities;

namespace Catalog.Api.Repositories
{
    public interface IProductRepository
    {
        Task CreateProduct(Product product);
        Task<IEnumerable<Product>> GetProducts();
        Task<bool> DeleteProduct(string id);
        Task<bool> UpdateProduct(Product product);
        Task<Product> GetProduct(string id);
    }
}