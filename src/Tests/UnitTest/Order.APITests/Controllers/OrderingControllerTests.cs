//using MassTransit.Mediator;
//using MediatR;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Ordering.API;
//using Ordering.API.Controllers;
//using EventBus.Messages.Common;
//using Microsoft.AspNetCore.Mvc;
//using Ordering.Application.Features.Orders.Commands.DeleteOrder;
//using Ordering.Application.Features.Orders.Commands.UpdateOrder;
//using Ordering.Application.Features.Orders.Queries.GetOrdersList;
//using Ordering.Application.Features.Orders.Queries;
//using System.Net;
//using TestData;

//namespace Order.APITests.Controllers;

//    public class OrderingControllerTests
//    {
//        private readonly Mock<IMediator> _mediatorMock;
//        private readonly OrderingController _controller;

//        public OrderingControllerTests()
//        {
//            _mediatorMock = new Mock<IMediator>();
//            _controller = new OrderingController(_mediatorMock.Object);
//        }

//        [Fact]
//        public async Task Orders_ValidUserName_ReturnsOkResultWithOrders()
//        {
//            // Arrange
//            string userName = "testuser";
//            var query = new GetOrdersListQuery(userName);
//            var orders = new List<OrderData> { /* create some test orders */ };
//            _mediatorMock.Setup(m => m.Send(query, default)).ReturnsAsync(orders);

//            // Act
//            var result = await _controller.Orders(userName);

//            // Assert
//            var okResult = Assert.IsType<ActionResult<ResponseMessage<IEnumerable<OrderQueryModel>>>>(result);
//            Assert.Equal(orders, okResult.Value.Data);
//            Assert.Equal(HttpStatusCode.OK.ToString(), okResult.Value.Message);
//        }

//        [Fact]
//        public async Task Orders_InvalidUserName_ReturnsNotFoundResult()
//        {
//            // Arrange
//            string userName = "nonexistentuser";
//            var query = new GetOrdersListQuery(userName);
//            var emptyOrders = Enumerable.Empty<OrderQueryModel>();
//            _mediatorMock.Setup(m => m.Send(query, default)).ReturnsAsync(emptyOrders);

//            // Act
//            var result = await _controller.Orders(userName);

//            // Assert
//            var notFoundResult = Assert.IsType<ActionResult<ResponseMessage<IEnumerable<OrderQueryModel>>>>(result);
//            Assert.Equal(HttpStatusCode.NotFound.ToString(), notFoundResult.Value.Message);
//        }

//        [Fact]
//        public async Task Order_ValidOrderId_ReturnsOkResultWithOrder()
//        {
//            // Arrange
//            int orderId = 123;
//            var query = new GetOrder(orderId);
//            var order = new OrderQueryModel { /* create a test order */ };
//            _mediatorMock.Setup(m => m.Send(query, default)).ReturnsAsync(order);

//            // Act
//            var result = await _controller.Order(orderId);

//            // Assert
//            var okResult = Assert.IsType<ActionResult<ResponseMessage<OrderQueryModel>>>(result);
//            Assert.Equal(order, okResult.Value.Data);
//            Assert.Equal(HttpStatusCode.OK.ToString(), okResult.Value.Message);
//        }

//        [Fact]
//        public async Task Order_InvalidOrderId_ReturnsNotFoundResult()
//        {
//            // Arrange
//            int orderId = 999; // non-existent order ID
//            var query = new GetOrder(orderId);
//            _mediatorMock.Setup(m => m.Send(query, default)).ReturnsAsync((OrderQueryModel)null);

//            // Act
//            var result = await _controller.Order(orderId);

//            // Assert
//            var notFoundResult = Assert.IsType<ActionResult<ResponseMessage<OrderQueryModel>>>(result);
//            Assert.Equal(HttpStatusCode.NotFound.ToString(), notFoundResult.Value.Message);
//        }

//        [Fact]
//        public async Task UpdateOrder_ValidCommand_ReturnsNoContentResult()
//        {
//            // Arrange
//            var command = new UpdateOrderCommand { /* create a test command */ };

//            // Act
//            var result = await _controller.UpdateOrder(command);

//            // Assert
//            var noContentResult = Assert.IsType<NoContentResult>(result);
//            Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
//        }

//        [Fact]
//        public async Task DeleteOrder_ValidOrderId_ReturnsNoContentResult()
//        {
//            // Arrange
//            int orderId = 123;
//            var command = new DeleteOrderCommand { OrderId = orderId };

//            // Act
//            var result = await _controller.DeleteOrder(orderId);

//            // Assert
//            var noContentResult = Assert.IsType<NoContentResult>(result);
//            Assert.Equal((int)HttpStatusCode.NoContent, noContentResult.StatusCode);
//        }
//    }


