using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.API;
using Ordering.API.Controllers;
using EventBus.Messages.Common;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Application.Features.Orders.Queries;
using System.Net;
using TestData;



namespace Order.APITests.Controllers;

public class OrderControllerTests
{
   
    private readonly Mock<IMediator> _mockMediator;
    private readonly OrderingController _orderController;

    public OrderControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _orderController = new OrderingController(_mockMediator.Object);
    }

    [Fact]
    public void GetOrder_WithValidUsername_ReturnsOrder()
    {
        // Arrange
        string username = "testUser";
        var expectedOrder = new OrderData
        {
            OrderId = 1,
            UserId = "123456",
            UserName = "Test User",
            OrderItems = new List<OrderItem>
            {
                new OrderItem { ProductId = 1, Quantity = 2 },
                new OrderItem { ProductId = 2, Quantity = 1 }
            }
        };

        _mockMediator.Setup(mediator => mediator.Send(username).Returns(/* your expected result */);


        // Act
        var result = _orderController.Orders(username);

        // Assert
        Assert.Equal(expectedOrder, result);
    }

    [Fact]
    public void GetOrder_WithInvalidUsername_ReturnsNotFound()
    {
        // Arrange
        string invalidUsername = "invalidUser";
        Order nullOrder = null;

        _mockBasketService.Setup(service => service.GetOrder(invalidUsername)).Returns(nullOrder);

        // Act
        var result = _orderController.GetOrder(invalidUsername);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void PlaceOrder_WithValidOrderDto_ReturnsOk()
    {
        // Arrange
        var orderDto = new OrderDto
        {
            UserId = "123456",
            UserName = "Test User",
            OrderItems = new List<OrderItemDto>
            {
                new OrderItemDto { ProductId = 1, Quantity = 2 },
                new OrderItemDto { ProductId = 2, Quantity = 1 }
            }
        };

        // Mock the Mediator behavior
        _mockMediator.Setup(mediator => mediator.Send(It.IsAny<PlaceOrderCommand>())).Returns(/* your expected result */);

        // Act
        var result = _orderController.PlaceOrder(orderDto);

        // Assert
        Assert.NotNull(result);
        // Additional assertions as per your requirements
    }

    [Fact]
    public void PlaceOrder_WithInvalidOrderDto_ReturnsBadRequest()
    {
        // Arrange
        OrderDto invalidOrderDto = null;

        // Act
        var result = _orderController.PlaceOrder(invalidOrderDto);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
}




