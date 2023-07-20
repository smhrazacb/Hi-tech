using IdentityModel.Client;
using System.Net.Http.Headers;

namespace ShoppingAggregator.Infrastructure;

public class HttpClientAuthorizationDelegatingHandler
        : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authorizationHeader = _httpContextAccessor.HttpContext
            .Request.Headers["Authorization"];

        if (!string.IsNullOrWhiteSpace(authorizationHeader))
        {
            request.Headers.Add("Authorization", new List<string>() { authorizationHeader });
        }

        var token = await GetTokenAsync();

        if (token != null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }

    async Task<string> GetTokenAsync()
    {// Retrieve the OpenIddict server configuration document containing the endpoint URLs.
        var client = new HttpClient();
        var configuration = await client.GetDiscoveryDocumentAsync("http://localhost:8000/");
        if (configuration.IsError)
        {
            throw new Exception($"An error occurred while retrieving the configuration document: {configuration.Error}");
        }

        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
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
