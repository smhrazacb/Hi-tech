namespace EventBus.Messages.Events
{
    public class IntegrationBaseEvent
    {
        public IntegrationBaseEvent()
        {
            OrderId = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public IntegrationBaseEvent(Guid id, DateTime createDate)
        {
            OrderId = id;
            CreationDate = createDate;
        }

        public Guid OrderId { get; private set; }

        public DateTime CreationDate { get; private set; }
    }
}
