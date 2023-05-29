using Basket.API.Entities;

namespace Basket.API.Services.Interfaces
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string userid);
        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
        Task DeleteBasket(string userid);
    }
}
