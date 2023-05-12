using ShoppingAggregator.Models;
using System.Threading.Tasks;

namespace ShoppingAggregator.Services.Interfaces
{
    public interface IBasketService
    {
        Task<BasketModel> GetBasket(string userName);
    }
}
