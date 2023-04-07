namespace EventBus.Messages.Events
{
    public class BasketCheckoutEvent : IntegrationBaseEvent
    {
        public Guid UserId { get; set; }
        public IEnumerable<ProductEvent> ShoppingItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
