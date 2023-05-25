using EventBus.Messages.Common;
using ShoppingAggregator.Extensions;
using ShoppingAggregator.Models;
using ShoppingAggregator.Services.Interfaces;

namespace ShoppingAggregator.Services
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
