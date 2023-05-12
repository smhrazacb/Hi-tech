using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }
        public async Task DeleteBasket(string userId)
        {
            await _redisCache.RemoveAsync(userId);
        }

        public async Task<ShoppingCart> GetBasket(string userid)
        {
            var basket = await _redisCache.GetStringAsync(userid);
            if (basket == null)
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }
        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.UserId, JsonConvert.SerializeObject(basket));
            return await GetBasket(basket.UserId);
        }
    }
}
