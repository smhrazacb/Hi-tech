﻿using EventBus.Messages.Models;

namespace EventBus.Messages.Events
{
    public class CatalogStockDelEvent : IntegrationBaseEvent
    {
        public int OrderId { get; set; }
        public IEnumerable<EventCartItem> ShoppingItems { get; set; }
    }
}

