using ServicesTest.Extensions;
using ServicesTest.Services.Interfaces;
using EventBus.Messages.Common;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;

namespace ServicesTest.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _client;

        public OrderService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ResponseMessage<IEnumerable<OrdersVm>>> GetOrdersByUserName(string UserId)
        {
            var response = await _client.GetAsync($"/api/v1/Order/{UserId}");
            return await response.ReadContentAs<ResponseMessage<IEnumerable<OrdersVm>>>();
        }
    }

}
