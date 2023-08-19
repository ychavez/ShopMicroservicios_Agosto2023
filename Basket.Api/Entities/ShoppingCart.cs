namespace Basket.Api.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; } = null!;
        public List<ShoppingCartItem> Items { get; set; } = new();
        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;

                foreach (var item in Items)
                {
                    totalPrice += item.Price;
                }
                return totalPrice;
            }
        }
        public ShoppingCart()
        {

        }

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }
    }
}
