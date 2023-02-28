using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class ProductEvent
    {
        public string Id { get; set; }
        public string NameWithShortDesc { get; set; }
        public decimal Price { get; set; }
        public uint Qty { get; set; }
    }
}
