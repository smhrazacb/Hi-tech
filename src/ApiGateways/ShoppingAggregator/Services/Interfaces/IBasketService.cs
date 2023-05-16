using EventBus.Messages.Common;
using ShoppingAggregator.Models;
using System.Threading.Tasks;

namespace ShoppingAggregator.Services.Interfaces
{
    public interface IBasketService
    {
        Task<ResponseMessage<BasketModel>> GetBasket(string userName);
    }
}
