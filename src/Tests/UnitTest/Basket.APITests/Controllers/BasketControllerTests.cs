using Xunit;
using Basket.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Basket.API.Repositories.Interfaces;
using Basket.APITests.TestData;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Basket.API.Mapping;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using EventBus.Messages.Common;
using EventBus.Messages.Events;

namespace Basket.API.Controllers.Tests
{
    public class BasketControllerTests
    {
        [Fact()]
        public async void CheckoutBasketTest()
        {
            Mock<IBasketRepository> mockRepor = new();
            mockRepor.Setup(repo => repo.GetBasket(BasketData.GetBasketData().UserId)).ReturnsAsync(BasketData.GetBasketData());

            Mock<ILogger<BasketController>> mockLogger = new();
            Mock<IPublishEndpoint> mockPublish = new();
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            BasketController pc = new BasketController(mockLogger.Object, mapper, mockPublish.Object, mockRepor.Object);
            var result = await pc.Checkout(BasketData.BasketCheckoutIdsDtoDummyData());
      
            // generate unit test         
            result.Result.Should().BeOfType<AcceptedResult>().Which.StatusCode.Should().Be(202);
            result.Result.Should().BeOfType<AcceptedResult>().Which.Value.Should()
                .BeOfType<BasketCheckoutEvent>().Which.Id.Should().NotBeEmpty();
        }
    }
}