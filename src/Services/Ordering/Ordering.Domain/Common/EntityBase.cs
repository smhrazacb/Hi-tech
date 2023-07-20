using Ordering.Domain.Entities;

namespace Ordering.Domain.Common
{
    public abstract class EntityBase
    {
        public int OrderId { get; protected set; }
        public string UserId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
        public virtual IEnumerable<OrderItem> ShoppingItems { get; set; }
        public virtual IList<OrderStatus> OrderStatuses { get; set; }
    }
}
