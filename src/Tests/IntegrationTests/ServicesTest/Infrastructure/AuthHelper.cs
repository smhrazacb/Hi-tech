using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesTest.Infrastructure
{
    public static class AuthHelper
    {
        public static async Task<string> GetTokenAsync()
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
                Scope = "openid email profile offline_access order_api basket_api catalog_api webhook_api",
                ClientId = "TestId",
                ClientSecret = "secret", 
            });

            if (tokenResponse.IsError)
            {
                throw new Exception($"An error occurred while retrieving an access token: {tokenResponse.Error}");
            }

            return tokenResponse.AccessToken;
        }
    }
}
