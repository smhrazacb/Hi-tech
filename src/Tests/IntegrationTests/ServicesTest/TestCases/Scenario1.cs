using Catalog.API.Data;
using Catalog.API.Entities.Dtos;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ServicesTest.Extensions;
using ServicesTest.Services;
using static MassTransit.ValidationResultExtensions;

namespace ServicesTest.TestCases
{
    public class Scenario1
    {
        private readonly CatalogService _fixture;
        List<string> productId = new();
        public Scenario1()
        {
            _fixture = new CatalogService(new CatalogWebApplicationFactory<Catalog.API.Startup>());
        }
        [Fact]
        public async void Scenario1_Test()
        {
            await TesProduct_UploadCSV_Valid();

            foreach (var id in productId)
                await Product_DeleteProduct_Valid(id);

            await Product_GetCountWitCategory_Valid();
        }
        public async Task TesProduct_UploadCSV_Valid()
        {
            // Arrange
            string url = "/api/v1/AdminProduct/";
            MultipartFormDataContent formdata = new MultipartFormDataContent();
            formdata.Add(new StringContent(TestData.ProductData.CSVFileContent()), "file", "file.csv");
            // Act
            var response1 = await _fixture.client.PostAsync(url, formdata);
            var result1 = response1.ReadContentAs<CSVDto>();
            await Console.Out.WriteLineAsync("");
            //Assert
            response1.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result1.Result.Should().BeOfType<CSVDto>().Which.NewProducts.Count().Should().Be(2);
            result1.Result.Should().BeOfType<CSVDto>().Which.UpdateProducts.Count().Should().Be(0);
            result1.Result.Should().BeOfType<CSVDto>().Which.DuplicatePartNumbers.Count().Should().Be(0);
            result1.Result.Should().BeOfType<CSVDto>().Which.InvalidEntries.Count().Should().Be(0);
            productId.Add(result1.Result.NewProducts.First().Id);
            productId.Add(result1.Result.NewProducts.Last().Id);
        }
        public async Task Product_DeleteProduct_Valid(string id)
        {
            // Arrange
            string url3 = $"/api/v1/AdminProduct/{id}";

            // Act
            var response3 = await _fixture.client.DeleteAsync(url3);

            // Assert
            response3.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
        }
        public async Task Product_GetCountWitCategory_Valid()//
        {
            // Arrange
            var url2 = "/api/v1/Products";

            // Act
            var response2 = await _fixture.client.GetAsync(url2);
            var result2 = response2.ReadContentAs<IEnumerable<CategoryWithCount>>();

            // Assert
            response2.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result2.Result.Should().HaveCount(4);

        }
    }
}
