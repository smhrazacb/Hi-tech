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

        public async Task<IEnumerable<Order>> GetOrdersByShoppingCart(string userid)
        {
            var orderList = await _dbContext.Orders
                                 .Where(o => o.UserId == userid)
                                 .ToListAsync();
            return orderList;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string createdBy)
        {
            var orderList =
                await _dbContext
                .Orders
                .Include(orderList => orderList.ShoppingItems)
                .Where(o => o.UserId == createdBy)
                .ToListAsync();
            return orderList;
        }
    }
}
