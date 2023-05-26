using Basket.API.Services.Interfaces;
using Basket.API.Services;
using Catalog.API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Basket.API.Services.Services;
using Moq;
using TestData;
using Microsoft.AspNetCore.Authentication;
using ServicesTest.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ServicesTest.Services
{
    public class BasketWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.Replace(ServiceDescriptor.Scoped(_ =>
                {
                   var _IIdentityService = new Mock<IIdentityService>();
                    _IIdentityService.Setup(Object => Object.GetUserIdentity()).Returns(BasketData.GetBasketData().UserId);
                    return _IIdentityService.Object;
                }));

            });
            builder.UseEnvironment("Development");
        }
    }
}