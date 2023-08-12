using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public CatalogController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await productRepository.GetProducts());
        }

        [HttpPost]
        public async Task<ActionResult> CreatePoduct([FromBody] Product product)
        {
            await productRepository.CreateProduct(product);
            return Ok();
        }

    }
}
