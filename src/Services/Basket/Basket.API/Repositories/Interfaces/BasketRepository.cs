using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories.Interfaces
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public async Task DeleteBasket(string id)
        {
           await _redisCache.RemoveAsync(id);
        }

        public async Task<ShoppingCart> GetBasket(string id)
        {
            var basket = await _redisCache.GetStringAsync(id);
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }
        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.Id, JsonConvert.SerializeObject(basket));
            return await GetBasket(basket.Id);
        }
    }
}
