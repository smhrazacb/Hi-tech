using Moq;
using Ordering.API.Controllers;
using EventBus.Messages.Common;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Application.Features.Orders.Queries;
using TestData;
using Ordering.API.Services;
using FluentAssertions;
using MediatR;
using System.Net;
using System.Collections.Generic;

namespace Order.APITests.Controllers;

public class OrderControllerTests
{
    private readonly Mock<IIdentityService> _IIdentityService;
    private readonly Mock<IMediator> _mockMediator;

    public OrderControllerTests()
    {
        _IIdentityService = new Mock<IIdentityService>();
        _mockMediator = new Mock<IMediator>();
    }

    [Fact]
    public void DeleteOrder_WithOrderId_ReturnNoContent()
    {
        //Arrange
        var dataValid = OrderData.Orders();
        _mockMediator.Setup(x => x.Send(It.IsAny<GetOrdersListQuery>(), default))
                  .Returns(Task.FromResult(dataValid));

        //Act

        var _orderController = new OrderingController(_mockMediator.Object);
        var result = _orderController.DeleteOrder(dataValid.FirstOrDefault().OrderId);

        //Assert

        result.Result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public void UpdateOrder_ReturnNoContent()
    {
        //Arrange
        var dataValid = OrderData.Orders();
        _mockMediator.Setup(x => x.Send(It.IsAny<UpdateOrderCommand>(), default))
                     .Returns(Task.FromResult(Unit.Value));

        //Act

        var _orderController = new OrderingController(_mockMediator.Object);
        var result = _orderController.UpdateOrder(new UpdateOrderCommand());

        //Assert

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<NoContentResult>();


    }

    [Fact]
    public void Orders_WithUserName_ReturnOrder()
    {
        //Arrange

        _IIdentityService
        .Setup(Object => Object.GetUserName())
        .Returns("TestUser");

        var dataValid = OrderData.Orders(); 
        _mockMediator.Setup(x => x.Send(It.IsAny<GetOrdersListQuery>(), default))
                  .Returns(Task.FromResult(dataValid));

        //Act

        var _orderController = new OrderingController(_mockMediator.Object);
        var result = _orderController.Orders(dataValid.FirstOrDefault().UserId);

        //Assert

        result.Result
                 .Should().NotBeNull();
    }

    [Fact]
    public void Orders_IfEmpty_ReturnsNotFoundResponse()
    {
        //Arrange
        var dataValid = Enumerable.Empty<OrderQueryModel>();
        _mockMediator.Setup(x => x.Send(It.IsAny<GetOrdersListQuery>(), default))
                     .Returns(Task.FromResult(dataValid.ToList()));

        //Act
        var _orderController = new OrderingController(_mockMediator.Object);
        var result = _orderController.Orders("Test");

        //Assert
        Assert.Equal(null, result.Result.Value.Data);

    }

    [Fact]
    public void Order_WithOrderId_ReturnOrder()
    {
        //Arrange
        var dataValid = OrderData.Orders();
        _mockMediator.Setup(x => x.Send(It.IsAny<GetOrder>(), default))
                  .Returns(Task.FromResult(dataValid.FirstOrDefault()));

        //Act

        var _orderController = new OrderingController(_mockMediator.Object);
        var result = _orderController.Order(dataValid.FirstOrDefault().OrderId);

        //Assert


        result.Result.Value.Should().BeOfType<ResponseMessage<OrderQueryModel>>();
    }

    [Fact]
    public void Order_IfOrdersNull_ReturnNotFound()
    {
        //Arrange
        _mockMediator.Setup(x => x.Send(It.IsAny<GetOrder>(), default))
                     .Returns(Task.FromResult<OrderQueryModel>(null));

        //Act

        var _orderController = new OrderingController(_mockMediator.Object);
        var result = _orderController.Order(1);

        //Assert
        Assert.Equal(null, result.Result.Value.Data);
    }
}


