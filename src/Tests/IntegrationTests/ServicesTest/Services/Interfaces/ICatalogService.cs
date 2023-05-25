using Catalog.API.Entities;
using EventBus.Messages.Common;

namespace ServicesTest.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<ResponseMessage<Category>> GetCatalog(string id);
    }
}
