using ShoppingAggregator.Extensions;
using ShoppingAggregator.Models;
using ShoppingAggregator.Services.Interfaces;
using IdentityModel.Client;
using System.Net.Http.Headers;
using EventBus.Messages.Common;

namespace ShoppingAggregator.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _client;

        public OrderService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ResponseMessage<IEnumerable<OrderResponseModel>>> GetOrdersByUserName(string UserId)
        {
            var response = await _client.GetAsync($"/api/v1/Order/{UserId}");
            return await response.ReadContentAs<ResponseMessage<IEnumerable<OrderResponseModel>>>();
        }
    }

}
