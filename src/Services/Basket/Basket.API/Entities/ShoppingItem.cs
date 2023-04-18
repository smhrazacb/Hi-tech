namespace Basket.API.Entities
{
    public class ShoppingItem
    {
        public string ProductId { get; set; }
        public string ProductNameShortdesc { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal OldUnitPrice { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }
    }
}
