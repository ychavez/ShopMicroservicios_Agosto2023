using Basket.Api.Entities;

namespace Basket.Api.Repositories
{
    public interface IBasketRepository
    {
        Task DeleteBasket(string username);

        Task<ShoppingCart?> GetBasket(string username);

        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
    }
}
