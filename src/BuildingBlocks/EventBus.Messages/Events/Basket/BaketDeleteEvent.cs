namespace EventBus.Messages.Events.Basket
{
    public class BasketDeleteEvent : IntegrationBaseEvent
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
    }
}
