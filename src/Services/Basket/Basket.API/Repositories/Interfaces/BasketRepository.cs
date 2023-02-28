using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;

namespace Basket.API.Repositories.Interfaces
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }
        public async Task DeleteBasket(Guid guid)
        {
            await _redisCache.RemoveAsync(guid.ToString());
        }

        public async Task<ShoppingCart> GetBasket(Guid guid)
        {
            var basket = await _redisCache.GetStringAsync(guid.ToString());
            if (basket == null)
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }
        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.ShoppingCartId.ToString(), JsonConvert.SerializeObject(basket));
            return await GetBasket(basket.ShoppingCartId);
        }
    }
}
