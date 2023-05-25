using Catalog.API.Entities;
using EventBus.Messages.Common;
using ServicesTest.Extensions;
using ServicesTest.Services.Interfaces;

namespace ServicesTest.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;

        public CatalogService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ResponseMessage<Category>> GetCatalog(string id)
        {
            var response = await _client.GetAsync($"/api/v1/Products/{id}");
            return await response.ReadContentAs<ResponseMessage<Category>>();
        }
    }
}
