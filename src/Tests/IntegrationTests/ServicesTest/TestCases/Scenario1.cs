using AutoMapper;
using Basket.API.Entities;
using Basket.API.Entities.Dtos;
using Catalog.API.Entities;
using Catalog.API.Entities.Dtos;
using EventBus.Messages.Common;
using EventBus.Messages.Events.Basket;
using FluentAssertions;
using MongoDB.Driver;
using Newtonsoft.Json;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries;
using Ordering.Domain.Entities;
using RabbitMQ.Client;
using ServicesTest.Extensions;
using ServicesTest.Mapper;
using ServicesTest.Services;
using System.Text;
using TestData;
using Webhooks.API.Controllers;
using Webhooks.API.Entities;

namespace ServicesTest.TestCases
{
    public class Scenario1 : IDisposable
    {
        private readonly WebhookService _WebhookService;
        private readonly CatalogService _Catalogfixture;
        private readonly BasketService _Basketfixture;
        private readonly OrderService _Orderfixture;
        int retry = 0;

        List<string> productId = new();
        public Scenario1()
        {
            _WebhookService = new WebhookService(new WebhookWebApplicationFactory<Webhooks.API.Startup>());
            _Catalogfixture = new CatalogService(new CatalogWebApplicationFactory<Catalog.API.Startup>());
            _Basketfixture = new BasketService(new BasketWebApplicationFactory<Basket.API.Startup>());
            _Orderfixture = new OrderService(new OrderWebApplicationFactory<Ordering.API.Startup>());
            QueueCleanup();
        }
        [Fact]
        public async void Scenario_checkout_Valid()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            var newproducts = await Product_UploadCSV_Valid();

            var userid = await Basket_CreateUpdate_Valid(TestData.BasketData.GetBasketData(newproducts));
            retry = 0;
            var mycart = await Basket_Get_Valid(userid);

            //Register Order Status Change Webhook
            await Webhook_SubscribeWebhook_Valid(TestData.WebhookData
                .GetWebhookSubscriptionRequestData("https://webhook.site/f5c279bf-05c5-42ea-9a29-c5880dfcf04b",
                WebhookType.OrderStatus.ToString()));

            var checkoutEvent = await Basket_Checkout_Valid(TestData.BasketData.BasketCheckoutIdsDtoDummyData(mycart.UserId));
            retry = 0;
            var orders = await Order_GetOrders_Valid(checkoutEvent.UserId);

            foreach (var order in orders)
            {
                foreach (var shoppingItem in order.ShoppingItems)
                {
                    retry = 0;
                    var product =
                        await Product_VerifyStock_Valid(shoppingItem.ProductId, 2);
                }
            }
            //retry = 0;
            //await Order_Update_Valid(orders.FirstOrDefault());

            retry = 0;
            await Order_GetOrder_Valid(orders.FirstOrDefault().OrderId, EOrderStatus.Confirmed);

            await Console.Out.WriteLineAsync("");
            //await Product_GetCountWitCategory_Valid();
            //await Basket_Delete_Valid(userid);

            //foreach (var id in newproducts)
            //    await Product_DeleteProduct_Valid(id);

        }

        public async Task<OrderQueryModel> Order_GetOrder_Valid(int orderId, EOrderStatus orderstatus)
        {
            // Arrange
            string url = $"/api/v1/Ordering/Order/{orderId}";
            // Act
            var response = await _Orderfixture.client.GetAsync(url);
            var result = await response.ReadContentAs<ResponseMessage<OrderQueryModel>>();
            if (result.Data.OrderStatuses.LastOrDefault().Status != EOrderStatus.Confirmed)
            {
                await Task.Delay(5000);
                retry++;
                if (retry < 15)
                    return await Order_GetOrder_Valid(orderId, orderstatus);
            }
            // Assert
            response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result.Succeeded.Should().Be(true);
            result.Data.OrderId.Should().Be(orderId);
            result.Data.OrderStatuses.LastOrDefault().Status.Should().Be(orderstatus);
            return result.Data;
        }
        public async Task<bool> Order_Update_Valid(OrderQueryModel order)
        {
            // Arrange
            //auto mapper configuration
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            }).CreateMapper();
            order.OrderStatuses = new List<GetOrderStatus>() { new GetOrderStatus()
            {
                Status=EOrderStatus.Confirmed,
                UpdatedBy =order.UserId
            }};
            var updateOrderCommand = mapper.Map<UpdateOrderCommand>(order);
            string url = $"/api/v1/Ordering";
            var content = new StringContent(JsonConvert.SerializeObject(updateOrderCommand), Encoding.UTF8, "application/json");

            // Act
            var response = await _Orderfixture.client.PutAsync(url, content);

            // Assert
            response.Should().HaveStatusCode(System.Net.HttpStatusCode.NoContent);
            return true;
        }
        public async Task<IEnumerable<OrderQueryModel>> Order_GetOrders_Valid(string userid)
        {
            // Arrange
            string url = $"/api/v1/Ordering/Orders/{userid}";
            // Act
            var response = await _Orderfixture.client.GetAsync(url);
            var result = await response.ReadContentAs<ResponseMessage<IEnumerable<OrderQueryModel>>>();
            if (!result.Succeeded)
            {
                await Task.Delay(5000);
                retry++;
                if (retry < 15)
                    return await Order_GetOrders_Valid(userid);
            }
            // Assert
            response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result.Succeeded.Should().Be(true);
            result.Data.FirstOrDefault().UserId.Should().Be(TestData.BasketData.GetBasketData().UserId);
            return result.Data;
        }
        public async Task<BasketCheckoutEvent> Basket_Checkout_Valid(BasketCheckoutIdsDto data)
        {
            // Arrange

            string url = $"/api/v1/Basket/Checkout";
            var content = new StringContent(JsonConvert
                .SerializeObject(data), Encoding.UTF8, "application/json");

            // Act
            var response = await _Basketfixture.client.PostAsync(url, content);
            var result = await response.ReadContentAs<BasketCheckoutEvent>();

            // Assert
            response.Should().HaveStatusCode(System.Net.HttpStatusCode.Accepted);
            result.UserId.Should().Be(data.UserId);
            return result;
        }
        public async Task<string> Basket_CreateUpdate_Valid(ShoppingCart shoppingCart)
        {
            // Arrange
            string url = $"/api/v1/Basket/";
            var content = new StringContent(JsonConvert.SerializeObject(shoppingCart), Encoding.UTF8, "application/json");

            // Act
            var response = await _Basketfixture.client.PutAsync(url, content);
            var result = await response.ReadContentAs<ResponseMessage<ShoppingCart>>();

            // Assert
            response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result.Succeeded.Should().Be(true);
            result.Data.Should().BeOfType<ShoppingCart>().Which.UserId.Should().Be(shoppingCart.UserId);
            return result.Data.UserId;
        }
        public async Task<ShoppingCart> Basket_Get_Valid(string userid)
        {
            // Arrange
            string url = $"/api/v1/Basket/{userid}";
            // Act
            var response = await _Basketfixture.client.GetAsync(url);
            var result = await response.ReadContentAs<ResponseMessage<ShoppingCart>>();
            if (!result.Succeeded)
            {
                await Task.Delay(5000);
                retry++;
                if (retry < 15)
                    return await Basket_Get_Valid(userid);
            }
            // Assert
            response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result.Succeeded.Should().Be(true);
            result.Data.Should().BeOfType<ShoppingCart>().Which.UserId.Should().Be(TestData.BasketData.GetBasketData().UserId);
            return result.Data;
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
            var result1 = await response1.ReadContentAs<CSVDto>();
            await Console.Out.WriteLineAsync("");
            //Assert
            response1.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result1.Should().BeOfType<CSVDto>().Which.NewProducts.Count().Should().Be(2);
            result1.Should().BeOfType<CSVDto>().Which.UpdateProducts.Count().Should().Be(0);
            result1.Should().BeOfType<CSVDto>().Which.DuplicatePartNumbers.Count().Should().Be(0);
            result1.Should().BeOfType<CSVDto>().Which.InvalidEntries.Count().Should().Be(0);

            return result1.NewProducts;

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
        public async Task<Category> Product_VerifyStock_Valid(string id, uint qty)
        {
            // Arrange
            string url = $"/api/v1/Products/{id}";

            // Act
            var response = await _Catalogfixture.client.GetAsync(url);
            var result = await response.ReadContentAs<ResponseMessage<Category>>();
            if (result.Data.SubCategory.Product.Quantity != qty)
            {
               await Task.Delay(5000);
                retry++;
                if (retry < 15)
                    return await Product_VerifyStock_Valid(id, qty);
            }

            // Assert
            response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result.Succeeded.Should().Be(true);
            result.Data.Should().BeOfType<Category>().Which.Id.Should().Be(id);
            result.Data.SubCategory.Product.Quantity.Should().Be(qty);
            return result.Data;
        }
        public async Task<Category> Product_Get_Valid(string id)
        {
            // Arrange
            string url = $"/api/v1/Products/{id}";

            // Act
            var response = await _Catalogfixture.client.GetAsync(url);
            var result = await response.ReadContentAs<ResponseMessage<Category>>();


            // Assert
            response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result.Succeeded.Should().Be(true);
            result.Data.Should().BeOfType<Category>().Which.Id.Should().Be(id);
            return result.Data;
        }
        public async Task Product_GetCountWitCategory_Valid()//
        {
            // Arrange
            var url2 = "/api/v1/Products";

            // Act
            var response2 = await _Catalogfixture.client.GetAsync(url2);
            var result2 = await response2.ReadContentAs<IEnumerable<CategoryWithCount>>();

            // Assert
            response2.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result2.Should().HaveCount(4);
        }
        public async Task Webhook_SubscribeWebhook_Valid(WebhookSubscriptionRequest data)
        {
            // Arrange
            string url = "/api/v1/Webhooks/";
            var content = new StringContent(JsonConvert
                .SerializeObject(data), Encoding.UTF8, "application/json");

            // Act
            var response = await _WebhookService.client.PostAsync(url, content);
            var responseaction = await _WebhookService.client.GetAsync(response.Headers.Location.AbsoluteUri);
            var result = await responseaction.ReadContentAs<WebhookSubscription>();

            // Assert
            response.Should().HaveStatusCode(System.Net.HttpStatusCode.Created);
            responseaction.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result.UserId.Should().Be(BasketData.GetBasketData().UserId);
            result.Id.Should().Be(1);
        }
        public void Dispose()
        {
            _Orderfixture.factory.DropDatabase();
            _Catalogfixture.dropDatabase();
            _Basketfixture.dropKey();
            _WebhookService.factory.DropDatabase();
            QueueCleanup();
        }

        private static void QueueCleanup()
        {
            ConnectionFactory factory = new ConnectionFactory();

            factory.HostName = "localhost";
            factory.UserName = "guest";
            factory.Password = "guest";

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var properties = typeof(EventBusConstants).GetFields();
                    foreach (var item in properties)
                    {
                        channel.QueueDelete(item.GetValue(null).ToString());
                        channel.QueueDelete(item.GetValue(null).ToString()+"_error");
                    }
                }
            }
        }
    }
}
