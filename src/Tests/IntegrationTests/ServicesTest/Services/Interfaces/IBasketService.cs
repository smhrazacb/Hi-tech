using Basket.API.Entities;
using EventBus.Messages.Common;

namespace ServicesTest.Services.Interfaces
{
    public interface IBasketService
    {
        Task<ResponseMessage<ShoppingCart>> GetBasket(string userName);
    }
}
