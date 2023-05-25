using Basket.API.Entities;
using EventBus.Messages.Common;
using ServicesTest.Extensions;
using ServicesTest.Services.Interfaces;

namespace ServicesTest.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _client;

        public BasketService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ResponseMessage<ShoppingCart>> GetBasket(string UserId)
        {
            var response = await _client.GetAsync($"/api/v1/Basket/{UserId}");
            return await response.ReadContentAs<ResponseMessage<ShoppingCart>>();

        }
    }
}
