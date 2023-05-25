using EventBus.Messages.Common;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;

namespace ServicesTest.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseMessage<IEnumerable<OrdersVm>>> GetOrdersByUserName(string userName);
    }
}
