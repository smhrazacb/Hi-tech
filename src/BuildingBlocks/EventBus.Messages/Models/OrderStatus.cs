using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Models
{
    public enum EOrderStatus
    {
        Initiated,
        Confirmed,
        Shipped,
        Delivered,
        Cancelled,
    }
    public class EventOrderStatus
    {
        public EOrderStatus Status { get;}
        public DateTime DateTimeStamp { get; } = DateTime.UtcNow;
        public string UpdatedBy { get;}

        public EventOrderStatus(EOrderStatus status, string updatedBy)
        {
            Status = status;
            UpdatedBy = updatedBy;
        }
    }


}
