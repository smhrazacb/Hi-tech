namespace EventBus.Messages.Events
{
    public class OrderCompleteEvent : IntegrationBaseEvent
    {
        public int OrderId { get; set; }
        public Guid ShoppingCartId { get; set; }
    }
}
