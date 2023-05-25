using Castle.Core.Configuration;
using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.Entities.Dtos;
using EventBus.Messages.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ServicesTest.Extensions;
using ServicesTest.Services;
using static MassTransit.ValidationResultExtensions;

namespace ServicesTest.TestCases
{
    public class Cases : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;
        public Cases(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }
        public void Dispose()
        {
            var settingsOptions = _factory.Services.GetService<DbContextSettings>();
            var client = new MongoClient(settingsOptions.ConnectionString);
            client.DropDatabase(settingsOptions.DatabaseName);
        }

        [Fact]
        public async void Product_GetCountWitCategory_Valid()
        {
            // Arrange
            string url = new("/api/v1/Products");

            // Act
            var response = await _client.GetAsync(url);
            var result = response.ReadContentAs<IEnumerable<CategoryWithCount>>();

            // Assert
            response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result.Result.Should().HaveCount(4);
        }
        public async void Product_GetCoudntWitCategory_Valid()
        {
        }
    }
}
