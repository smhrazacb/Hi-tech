using Basket.API.Entities;

namespace Basket.API.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(Guid guid);
        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
        Task DeleteBasket(Guid guid);
    }
}
