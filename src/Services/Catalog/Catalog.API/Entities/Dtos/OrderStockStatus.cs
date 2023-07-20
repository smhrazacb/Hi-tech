using EventBus.Messages.Models;

namespace Catalog.API.Entities.Dtos
{
    public class OrderStockStatus
    {
        public EventCartItem OrderItem { get; set; }
        public string  Status { get; set; }

        public OrderStockStatus(EventCartItem orderItem, string status)
        {
            OrderItem = orderItem;
            Status = status;
        }
    }
}
