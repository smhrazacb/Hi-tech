using EventBus.Messages.Common;
using ShoppingAggregator.Models;

namespace ShoppingAggregator.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<ResponseMessage<Category>> GetCatalog(string id);
    }
}
