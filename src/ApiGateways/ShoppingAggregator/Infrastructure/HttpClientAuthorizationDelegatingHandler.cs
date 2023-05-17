using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using OpenIddict.Client;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingAggregator.Infrastructure
{
    public class HttpClientAuthorizationDelegatingHandler
         : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly HttpClient _client;

        public HttpClientAuthorizationDelegatingHandler(OpenIddictClientService service, IHttpContextAccessor httpContextAccesor, HttpClient client)
        {
            _httpContextAccesor = httpContextAccesor;
            _client = client;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorizationHeader = _httpContextAccesor.HttpContext
                .Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                request.Headers.Add("Authorization", new List<string>() { authorizationHeader });
            }

            var token = await GetToken();

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }

        async Task<string> GetToken()
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
            return tokenResponse.AccessToken;

        }
    }
}
