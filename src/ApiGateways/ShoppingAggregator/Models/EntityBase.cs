namespace ShoppingAggregator.Models
{
    public class BaseEntity
    {
        public string Id { get; set; }
        public DateTime ModifiedDate { get; private set; } = DateTime.Now;
    }
}
