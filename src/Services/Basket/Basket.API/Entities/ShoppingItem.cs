namespace Basket.API.Entities
{
    public class ShoppingItem
    {
        public string Id { get; set; }
        public string NameWithShortDesc { get; set; }
        public uint Qty { get; set; }
        public decimal Price { get; set; }
    }
}
