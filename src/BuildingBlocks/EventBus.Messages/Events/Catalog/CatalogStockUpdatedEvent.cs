using EventBus.Messages.Models;

namespace EventBus.Messages.Events.Catalog
{
    public class CatalogStockUpdatedEvent : IntegrationBaseEvent
    {
        public int OrderId { get; set; }
    }
}

