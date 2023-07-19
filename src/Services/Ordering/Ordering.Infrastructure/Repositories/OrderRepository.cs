using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserId(string userid)
        {
            return await _dbContext.Orders
                                 .Where(o => o.UserId == userid)
                                 .ToListAsync();
        }
        public async Task<Order> GetOrdersByOrderId(int orderid)
        {
            return await _dbContext.Orders
                                 .Where(o => o.OrderId == orderid)
                                 .Include(o => o.ShoppingItems)
                                 .Include(o => o.OrderStatuses)
                                 .FirstOrDefaultAsync();
                }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string createdBy)
        {
            var orderList =
                await _dbContext
                .Orders
                .Include(o => o.ShoppingItems)
                .Include(o => o.OrderStatuses)
                .Where(o => o.UserId == createdBy)
                .ToListAsync();
            return orderList;
        }
    }
}
