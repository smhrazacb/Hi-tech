namespace EventBus.Messages.Events
{
    public class IntegrationBaseEvent
    {
        public Guid Id { get; private set; }

        public DateTimeOffset CreationDate { get; private set; } = DateTimeOffset.UtcNow;
    }
}
