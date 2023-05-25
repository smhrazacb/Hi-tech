using EventBus.Messages.Common;
using ShoppingAggregator.Models;

namespace ShoppingAggregator.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseMessage<IEnumerable<OrderResponseModel>>> GetOrdersByUserName(string userName);
    }
}
