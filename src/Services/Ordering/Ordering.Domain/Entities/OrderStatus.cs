using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.Domain.Entities
{
    public class OrderStatus
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public int OrderId { get; private set; }
        public string Status { get; set; }
        public DateTimeOffset DateTimeStamp { get; set; } = DateTimeOffset.UtcNow;
    }
}
