namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string UserId { get; set; }
        public IEnumerable<ShoppingItem> ShoppingItems { get; set; }
    }
}
