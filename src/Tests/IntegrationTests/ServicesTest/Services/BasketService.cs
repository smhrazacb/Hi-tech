using Basket.API.Services;
using Basket.API.Services.Interfaces;
using Catalog.API.Data;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using ServicesTest.Infrastructure;
using System.Net.Http.Headers;

namespace ServicesTest.Services
{
    public class BasketService : IClassFixture<BasketWebApplicationFactory<Basket.API.Startup>>
    {
        public readonly HttpClient client;
        public readonly BasketWebApplicationFactory<Basket.API.Startup> factory;
        public BasketService(BasketWebApplicationFactory<Basket.API.Startup> factory)
        {
            this.factory = factory;
            client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });
            var token = AuthHelper.GetTokenAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Result);
            // old keys
            dropKey();
        }
        public async void dropKey()
        {
            var scope = factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var iBasketRepository = scope.ServiceProvider.GetRequiredService<IBasketRepository>();
            await iBasketRepository.DeleteBasket(TestData.BasketData.GetBasketData().UserId);
        }
    }
}
