using EventBus.Messages.Common;
using ShoppingAggregator.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingAggregator.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseMessage<IEnumerable<OrderResponseModel>>> GetOrdersByUserName(string userName);
    }
}
