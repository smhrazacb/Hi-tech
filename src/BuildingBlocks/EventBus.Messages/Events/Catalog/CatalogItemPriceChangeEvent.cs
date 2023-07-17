using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events.Catalog
{
    public class CatalogItemPriceChangeEvent : IntegrationBaseEvent
    {
        public string ProductId { get; set; }
        public decimal OldUnitPrice { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
