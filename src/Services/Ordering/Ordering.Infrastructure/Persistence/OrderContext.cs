using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Common;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
