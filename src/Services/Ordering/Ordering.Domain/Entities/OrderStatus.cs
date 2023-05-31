using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Entities
{
    public enum EOrderStatus
    {
        Initiated,
        Confirmed,
        Shipped,
        Delivered,
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
        public virtual Order Order { get; set; }
    }


}
