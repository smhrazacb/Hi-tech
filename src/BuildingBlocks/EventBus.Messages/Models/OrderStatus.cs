using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        public string Status { get; }
        public DateTime DateTimeStamp { get; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        public string ErrorMessage { get; set; }

        public EventOrderStatus(string status)
        {
            Status = status;
        }
        public EventOrderStatus(string status, string error)
        {
            Status = status;
            ErrorMessage = error;
        }
    }


}
