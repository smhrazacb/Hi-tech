using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Ordering.Domain.Entities
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EOrderStatus
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
    }
    public class OrderStatus
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public int OrderId { get; private set; }
        public EOrderStatus Status { get; set; }
        public DateTime DateTimeStamp { get; private set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; }
    }
}
