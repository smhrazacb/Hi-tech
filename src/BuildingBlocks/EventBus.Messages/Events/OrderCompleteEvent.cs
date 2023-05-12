namespace EventBus.Messages.Events
{
    public class OrderCompleteEvent : IntegrationBaseEvent
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
    }
}
