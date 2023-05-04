using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBus.Messages.Models;

namespace EventBus.Messages.Events
{
    public class CatalogStockDelEvent : IntegrationBaseEvent
    {
        public int OrderId { get; set; }
        public IEnumerable<EventCartItem> ShoppingItems { get; set; }
    }
}

