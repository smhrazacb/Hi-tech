namespace EventBus.Messages.Models
{
    public enum EventEOrderStatus
    {
        Initiated,
        Confirmed,
        Shipped,
        Delivered,
        Cancelled,
        Failed,
    }
    public class EventOrderStatus
    {
        public string Status { get; set; }
        public DateTimeOffset DateTimeStamp { get; set; } = DateTimeOffset.UtcNow;
        public string? ErrorMessage { get; set; }
    }


}
