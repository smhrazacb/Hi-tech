using Catalog.API.Data;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using ServicesTest.Infrastructure;
using System.Net.Http.Headers;

namespace ServicesTest.Services
{
    public class CatalogService : IClassFixture<CatalogWebApplicationFactory<Catalog.API.Startup>>
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
            var token = AuthHelper.GetTokenAsync().Result;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // old database
            dropDatabase();
        }
        public void dropDatabase()
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

    }
}
