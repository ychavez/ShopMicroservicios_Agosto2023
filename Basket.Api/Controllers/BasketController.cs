using Basket.Api.Entities;
using Basket.Api.Repositories;
using EventBus.Messages.Events;
using Inventory.grpc.Protos;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly ExistenceService.ExistenceServiceClient existenceService;
        private readonly IPublishEndpoint publishEndpoint;

        public BasketController(IBasketRepository basketRepository,
            ExistenceService.ExistenceServiceClient existenceService,
            IPublishEndpoint publishEndpoint)
        {
            this.basketRepository = basketRepository;
            this.existenceService = existenceService;
            this.publishEndpoint = publishEndpoint;
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

        [HttpPost("Checkout")]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await basketRepository.GetBasket(basketCheckout.UserName);

            if (basket is null)
                return BadRequest();

            var eventMessage = new BasketCheckoutEvent
            {
                UserName = basketCheckout.UserName,
                Address = basketCheckout.Address,
                FirstName = basketCheckout.FirstName,
                LastName = basketCheckout.LastName,
                PaymentMethod = basketCheckout.PaymentMethod,
                TotalPrice = basketCheckout.TotalPrice,
            };

            await publishEndpoint.Publish(eventMessage);

            await basketRepository.DeleteBasket(basketCheckout.UserName);

            return Accepted();
        }
    }
}
