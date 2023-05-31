﻿using EventBus.Messages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class OrderStatusChangeEvent : IntegrationBaseEvent
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public IList<EventOrderStatus> Statuses { get; set; }
    }
}
