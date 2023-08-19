using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache distributedCache;

        public BasketRepository(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task DeleteBasket(string username)
        {
            await distributedCache.RemoveAsync(username);
        }

        public async Task<ShoppingCart?> GetBasket(string username)
        {
            var basket = await distributedCache.GetStringAsync(username);

            if (basket is null)
                return null;

            return JsonSerializer.Deserialize<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await distributedCache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));

            return (await GetBasket(basket.UserName))!;
        }
    }
}
