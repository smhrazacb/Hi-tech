namespace ShoppingAggregator.Models
{
    public class BasketModel
    {
        public string UserId { get; set; }
        public IEnumerable<CatalogModel> ShoppingItems { get; set; } = new List<CatalogModel>();
        public decimal TotalPrice { get; set; }
    }
}
