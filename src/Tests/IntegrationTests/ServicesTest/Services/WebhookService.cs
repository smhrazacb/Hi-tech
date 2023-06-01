using Castle.Core.Configuration;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver.Core.Configuration;
using Npgsql;
using Ordering.Infrastructure.Persistence;
using ServicesTest.Infrastructure;
using System.Data.Common;
using System.Net.Http.Headers;

namespace ServicesTest.Services
{
    public class WebhookService : IClassFixture<WebhookWebApplicationFactory<Webhooks.API.Startup>>
    {
        public readonly HttpClient client;
        public readonly WebhookWebApplicationFactory<Webhooks.API.Startup> factory;
        public WebhookService(WebhookWebApplicationFactory<Webhooks.API.Startup> factory)
        {
            this.factory = factory;
            client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });
            var token = AuthHelper.GetTokenAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Result);
        }
        
    }
}
