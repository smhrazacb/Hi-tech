using ShoppingAggregator.Extensions;
using ShoppingAggregator.Models;
using ShoppingAggregator.Services.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShoppingAggregator.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _client;

        public BasketService(HttpClient client)
        {
            _client = client;
        }

        public async Task<BasketModel> GetBasket(string UserId)
        {
            var response = await _client.GetAsync($"/api/v1/Basket/{UserId}");
            return await response.ReadContentAs<BasketModel>();
        }        
    }
}
