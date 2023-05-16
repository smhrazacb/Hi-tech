using EventBus.Messages.Common;
using ShoppingAggregator.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingAggregator.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<ResponseMessage<Category>> GetCatalog(string id);
    }
}
