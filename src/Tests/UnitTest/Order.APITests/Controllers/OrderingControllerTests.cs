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
using Ordering.API.Services;
using FluentAssertions;

namespace Order.APITests.Controllers;

public class OrderControllerTests
{
    private readonly Mock<IIdentityService> _IIdentityService;
    private readonly Mock<IMediator> _mockMediator;
    private readonly OrderingController _orderController;

    public OrderControllerTests()
    {
        _IIdentityService = new Mock<IIdentityService>();
        _mockMediator = new Mock<IMediator>();
        _orderController = new OrderingController(_mockMediator.Object);
    }

    [Fact]
    public void GetOrders_ReturnOrder()
    {
        //Arrange
        _IIdentityService
        .Setup(Object => Object.GetUserName())
        .Returns("TestUser");

        var result = _orderController.Orders(OrderData.UserName);
        

        //Act

        //Assert



    }
    
}


