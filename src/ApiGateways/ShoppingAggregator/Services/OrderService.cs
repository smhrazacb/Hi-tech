using ShoppingAggregator.Extensions;
using ShoppingAggregator.Models;
using ShoppingAggregator.Services.Interfaces;
using IdentityModel.Client;
using System.Net.Http.Headers;

namespace ShoppingAggregator.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _client;

        public OrderService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string UserId)
        {
            // Retrieve the OpenIddict server configuration document containing the endpoint URLs.
            var configuration = await _client.GetDiscoveryDocumentAsync("http://localhost:8000/");
            if (configuration.IsError)
            {
                throw new Exception($"An error occurred while retrieving the configuration document: {configuration.Error}");
            }

            var tokenResponse = await _client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = configuration.TokenEndpoint,
                Scope = "openid email profile offline_access order_api basket_api catalog_api",
                ClientId = "shopping_aggrigator_server",
                ClientSecret = "secret"
            });

            if (tokenResponse.IsError)
            {
                throw new Exception($"An error occurred while retrieving an access token: {tokenResponse.Error}");
            }
            var response = await _client.GetAsync($"/api/v1/Order/{UserId}");
            return await response.ReadContentAs<List<OrderResponseModel>>();
        }
        async Task<string> GetResourceAsync(IServiceProvider provider, string token)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44385/api/message");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }

}
