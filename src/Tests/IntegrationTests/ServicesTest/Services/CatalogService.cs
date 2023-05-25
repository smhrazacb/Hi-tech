using Catalog.API.Data;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System.Net.Http.Headers;

namespace ServicesTest.Services
{
    public class CatalogService : IClassFixture<CatalogWebApplicationFactory<Catalog.API.Startup>>, IDisposable
    {
        public readonly HttpClient client;
        public readonly CatalogWebApplicationFactory<Catalog.API.Startup> factory;
        public CatalogService(CatalogWebApplicationFactory<Catalog.API.Startup> factory)
        {
            this.factory = factory;
            client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });
            var token = GetTokenAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Result);
            // old database
            dropDatabase();
        }
        void dropDatabase()
        {
            var settingsOptions = factory.Services.GetService<DbContextSettings>();
            var client = new MongoClient(settingsOptions.ConnectionString);
            var databases = client.ListDatabaseNames().ToList();
            foreach (var item in databases)
            {
                if (item == settingsOptions.DatabaseName)
                    client.DropDatabase(settingsOptions.DatabaseName);
            }
        }
        public void Dispose()
        {
            dropDatabase();
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
}
