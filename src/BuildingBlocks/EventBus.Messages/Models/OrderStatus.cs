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
        public string UpdatedBy { get; }
        public string ErrorMessage { get; set; }

        public EventOrderStatus(string updatedBy, string status)
        {
            Status = status;
            UpdatedBy = updatedBy;
        }
        public EventOrderStatus(string updatedBy, string status, string error)
        {
            Status = status;
            UpdatedBy = updatedBy;
            ErrorMessage = error;
        }
    }


}
