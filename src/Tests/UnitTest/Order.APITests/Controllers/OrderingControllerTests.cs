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
using Ordering.API.Services;
using FluentAssertions;
using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Basket.API.Entities;
using Ordering.Domain.Entities;
using StackExchange.Redis;
using System.ComponentModel;

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
    public void Order_WithOrderId_ReturnOrder()
    {
        //Arrange
        var dataValid = OrderData.Orders();
        _mockMediator.Setup(x => x.Send(It.IsAny<GetOrdersListQuery>(), default))
                  .Returns(Task.FromResult(dataValid));

        //Act

        var _orderController = new OrderingController(_mockMediator.Object);
        var result = _orderController.Order(dataValid.FirstOrDefault().OrderId);

        //Assert

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<ActionResult<ResponseMessage<OrderQueryModel>>>();

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


}


