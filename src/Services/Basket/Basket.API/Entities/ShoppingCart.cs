namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public Guid ShoppingCartId { get; set; } = Guid.NewGuid();
        public IEnumerable<ShoppingItem> ShoppingItems { get; set; }
    }
}
