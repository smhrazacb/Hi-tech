using Basket.API.Entities;
using Basket.API.Entities.Dtos;
using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.Entities.Dtos;
using EventBus.Messages.Common;
using EventBus.Messages.Events;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using ServicesTest.Extensions;
using ServicesTest.Services;
using System.Text;
using static MassTransit.ValidationResultExtensions;

namespace ServicesTest.TestCases
{
    public class Scenario1 : IDisposable
    {
        private readonly CatalogService _Catalogfixture;
        private readonly BasketService _Basketfixture;
        private readonly OrderService _Orderfixture;
        int retry = 0;

        List<string> productId = new();
        public Scenario1()
        {
            _Catalogfixture = new CatalogService(new CatalogWebApplicationFactory<Catalog.API.Startup>());
            _Basketfixture = new BasketService(new BasketWebApplicationFactory<Basket.API.Startup>());
            _Orderfixture = new OrderService(new OrderWebApplicationFactory<Ordering.API.Startup>());
        }
        [Fact]
        public async void Scenario_Scheckout_Valid()
        {

            var newproducts = await Product_UploadCSV_Valid();


            var userid = await Basket_CreateUpdate_Valid(TestData.BasketData.GetBasketData(newproducts));

            var mycart = await Basket_Get_Valid(userid);

            var checkoutEvent = await Basket_Checkout_Valid();
            retry = 0;
            var orders = await Order_Get_Valid();

            foreach (var order in orders)
            {
                foreach (var shoppingItem in order.ShoppingItems)
                {
                    retry = 0;
                    var product =
                        await Product_Get_Valid(shoppingItem.ProductId, 2);
                }
            }

            //await Product_GetCountWitCategory_Valid();
            //await Basket_Delete_Valid(userid);

            //foreach (var id in newproducts)
            //    await Product_DeleteProduct_Valid(id);

        }
        public async Task<IEnumerable<OrdersVm>> Order_Get_Valid()
        {
            // Arrange
            string url = $"/api/v1/Order/{TestData.BasketData.GetBasketData().UserId}";
            // Act
            var response = await _Orderfixture.client.GetAsync(url);
            var result = response.ReadContentAs<ResponseMessage<IEnumerable<OrdersVm>>>();
            if (!result.Result.Succeeded)
            {
                Task.Delay(5000);
                retry++;
                if (retry< 15)
                    return await Order_Get_Valid();
            }
            // Assert


            response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result.Result.Succeeded.Should().Be(true);
            result.Result.Data.FirstOrDefault().UserId.Should().Be(TestData.BasketData.GetBasketData().UserId);
            return result.Result.Data;
        }
        public async Task<BasketCheckoutEvent> Basket_Checkout_Valid()
        {
            // Arrange
            var data = TestData.BasketData.BasketCheckoutIdsDtoDummyData();
            string url = $"/api/v1/Basket/Checkout";
            var content = new StringContent(JsonConvert
                .SerializeObject(data), Encoding.UTF8, "application/json");

            // Act
            var response = await _Basketfixture.client.PostAsync(url, content);
            var result = response.ReadContentAs<BasketCheckoutEvent>();

            // Assert
            response.Should().HaveStatusCode(System.Net.HttpStatusCode.Accepted);
            result.Result.UserId.Should().Be(data.UserId);
            return result.Result;
        }
        public async Task<string> Basket_CreateUpdate_Valid(ShoppingCart shoppingCart)
        {
            // Arrange
            string url = $"/api/v1/Basket/";
            var content = new StringContent(JsonConvert.SerializeObject(shoppingCart), Encoding.UTF8, "application/json");

            // Act
            var response = await _Basketfixture.client.PutAsync(url, content);
            var result = response.ReadContentAs<ResponseMessage<ShoppingCart>>();

            // Assert
            response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result.Result.Succeeded.Should().Be(true);
            result.Result.Data.Should().BeOfType<ShoppingCart>().Which.UserId.Should().Be(shoppingCart.UserId);
            return result.Result.Data.UserId;
        }
        public async Task<ShoppingCart> Basket_Get_Valid(string userid)
        {
            // Arrange
            string url = $"/api/v1/Basket/{userid}";
            // Act
            var response = await _Basketfixture.client.GetAsync(url);
            var result = response.ReadContentAs<ResponseMessage<ShoppingCart>>();

            // Assert
            response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result.Result.Succeeded.Should().Be(true);
            result.Result.Data.Should().BeOfType<ShoppingCart>().Which.UserId.Should().Be(TestData.BasketData.GetBasketData().UserId);
            return result.Result.Data;
        }
        public async Task Basket_Delete_Valid(string userid)
        {
            // Arrange
            string url = $"/api/v1/Basket/{userid}";
            // Act
            var response = await _Basketfixture.client.DeleteAsync(url);

            // Assert
            response.Should().HaveStatusCode(System.Net.HttpStatusCode.NoContent);
        }


        public async Task<IEnumerable<Category>> Product_UploadCSV_Valid()
        {
            // Arrange
            string url = "/api/v1/AdminProduct/";
            MultipartFormDataContent formdata = new MultipartFormDataContent();
            formdata.Add(new StringContent(TestData.ProductData.CSVFileContent()), "file", "file.csv");
            // Act
            var response1 = await _Catalogfixture.client.PostAsync(url, formdata);
            var result1 = response1.ReadContentAs<CSVDto>();
            await Console.Out.WriteLineAsync("");
            //Assert
            response1.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result1.Result.Should().BeOfType<CSVDto>().Which.NewProducts.Count().Should().Be(2);
            result1.Result.Should().BeOfType<CSVDto>().Which.UpdateProducts.Count().Should().Be(0);
            result1.Result.Should().BeOfType<CSVDto>().Which.DuplicatePartNumbers.Count().Should().Be(0);
            result1.Result.Should().BeOfType<CSVDto>().Which.InvalidEntries.Count().Should().Be(0);

            return result1.Result.NewProducts;

        }
        public async Task Product_DeleteProduct_Valid(string id)
        {
            // Arrange
            string url3 = $"/api/v1/AdminProduct/{id}";

            // Act
            var response3 = await _Catalogfixture.client.DeleteAsync(url3);

            // Assert
            response3.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
        }
        public async Task<Category> Product_Get_Valid(string id, uint qty)
        {
            // Arrange
            string url = $"/api/v1/Products/{id}";

            // Act
            var response = await _Catalogfixture.client.GetAsync(url);
            var result = response.ReadContentAs<ResponseMessage<Category>>();
            if (result.Result.Data.SubCategory.Product.Quantity!=qty)
            {
                Task.Delay(5000);
                retry++;
                if (retry < 15)
                    return await Product_Get_Valid(id, qty);
            }

            // Assert
            response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result.Result.Succeeded.Should().Be(true);
            result.Result.Data.Should().BeOfType<Category>().Which.Id.Should().Be(id);
            result.Result.Data.SubCategory.Product.Quantity.Should().Be(qty);
            return result.Result.Data;
        }

        public async Task<Category> Product_Get_Valid(string id)
        {
            // Arrange
            string url = $"/api/v1/Products/{id}";

            // Act
            var response = await _Catalogfixture.client.GetAsync(url);
            var result = response.ReadContentAs<ResponseMessage<Category>>();


            // Assert
            response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result.Result.Succeeded.Should().Be(true);
            result.Result.Data.Should().BeOfType<Category>().Which.Id.Should().Be(id);
            return result.Result.Data;
        }
        public async Task Product_GetCountWitCategory_Valid()//
        {
            // Arrange
            var url2 = "/api/v1/Products";

            // Act
            var response2 = await _Catalogfixture.client.GetAsync(url2);
            var result2 = response2.ReadContentAs<IEnumerable<CategoryWithCount>>();

            // Assert
            response2.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result2.Result.Should().HaveCount(4);
        }
        public void Dispose()
        {
            _Orderfixture.factory.DropDatabase();
            _Catalogfixture.dropDatabase();
            _Basketfixture.dropKey();
        }
    }
}
