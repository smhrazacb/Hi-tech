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
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EventEOrderStatus
    {
        [EnumMember(Value = "initiated")]
        Initiated,
        [EnumMember(Value = "confirmed")]
        Confirmed,
        [EnumMember(Value = "shipped")]
        Shipped,
        [EnumMember(Value = "delivered")]
        Delivered,
        [EnumMember(Value = "cancelled")]
        Cancelled,
        [EnumMember(Value = "failed")]
        Failed,
    }
    public class EventOrderStatus
    {
        public EventEOrderStatus Status { get;}
        public DateTime DateTimeStamp { get; } = DateTime.UtcNow;
        public string UpdatedBy { get;}
        public string  ErrorMessage { get; set; }

        public EventOrderStatus( string updatedBy, EventEOrderStatus status)
        {
            Status = status;
            UpdatedBy = updatedBy;
        }
        public EventOrderStatus(string updatedBy, EventEOrderStatus status, string error)
        {
            Status = status;
            UpdatedBy = updatedBy;
            ErrorMessage = error;
        }
    }


}
