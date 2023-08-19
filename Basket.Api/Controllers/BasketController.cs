using Basket.Api.Entities;
using Basket.Api.Repositories;
using Inventory.grpc.Protos;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly ExistenceService.ExistenceServiceClient existenceService;

        public BasketController(IBasketRepository basketRepository, ExistenceService.ExistenceServiceClient existenceService)
        {
            this.basketRepository = basketRepository;
            this.existenceService = existenceService;
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await basketRepository.GetBasket(userName);

            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpDelete("{userName}")]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            await basketRepository.DeleteBasket(userName);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart shoppingCart)
        {
            foreach (var item in shoppingCart.Items)
            {
                var existence = await existenceService.CheckExistenceAsync(new ProductRequest { Id = item.ProductId });

                item.Quantity = item.Quantity > existence.PrductQty ? existence.PrductQty : item.Quantity;
            }

            await basketRepository.UpdateBasket(shoppingCart);
            return Ok(shoppingCart);
        }
    }
}
