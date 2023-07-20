using EventBus.Messages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events.Order
{
    public class OrderStatusChangedEvent : IntegrationBaseEvent
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public List<EventOrderStatus> OrderStatuses { get; set; } = new List<EventOrderStatus>();
    }
}
