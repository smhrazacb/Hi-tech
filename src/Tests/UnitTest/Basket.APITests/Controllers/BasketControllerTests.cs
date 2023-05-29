using Moq;
using Basket.API.Services.Interfaces;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Basket.API.Mapper;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using EventBus.Messages.Common;
using EventBus.Messages.Events;
using Basket.API.Services;
using Basket.API.Entities;
using TestData;

namespace Basket.API.Controllers.Tests
{
    public class BasketControllerTests
    {
        private readonly Mock<IIdentityService> _IIdentityService;
        private readonly Mock<ILogger<BasketController>> _ILogger;
        private readonly Mock<IPublishEndpoint> _IPublishEndpoint;
        private readonly Mock<IBasketRepository> _IBasketRepository;
        private readonly AutoMapper.Mapper _Mapper;

        public BasketControllerTests()
        {
            _IIdentityService = new Mock<IIdentityService>();
            _IIdentityService.Setup(Object => Object.GetUserIdentity()).Returns(BasketData.GetBasketData().UserId);
            _ILogger = new Mock<ILogger<BasketController>>();
            _IPublishEndpoint = new Mock<IPublishEndpoint>();
            _IBasketRepository = new Mock<IBasketRepository>();
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _Mapper = new AutoMapper.Mapper(configuration);
        }

        [Fact()]
        public async void CheckoutBasketTest()
        {
            //Arrange
            _IBasketRepository.Setup(repo => repo.GetBasket(BasketData.GetBasketData().UserId)).ReturnsAsync(BasketData.GetBasketData());

            //Act
            BasketController pc = new BasketController(_IIdentityService.Object, _ILogger.Object, _Mapper, _IPublishEndpoint.Object, _IBasketRepository.Object);
            var result = await pc.Checkout(BasketData.BasketCheckoutIdsDtoDummyData());

            // assert        
            result.Result.Should().BeOfType<AcceptedResult>().Which.StatusCode
                .Should().Be(202);
            result.Result
                .Should().BeOfType<AcceptedResult>().Which.Value
                .Should().BeOfType<BasketCheckoutEvent>().Which.Id
                .Should().NotBeEmpty();
        }

        // Basket updatetest metod     
        [Fact()]
        public async void BasketUpdate_Valid()
        {
            //Arrange
            var data = BasketData.GetBasketData();
            _IBasketRepository.Setup(repo => repo.UpdateBasket(It.IsAny<ShoppingCart>())).Returns(Task.FromResult(data));

            //Act
            BasketController pc = new BasketController(_IIdentityService.Object, _ILogger.Object, _Mapper, _IPublishEndpoint.Object, _IBasketRepository.Object);
            var result = await pc.UpdateBasket(BasketData.GetBasketData());

            // assert        
            result.Result
                .Should().BeOfType<OkObjectResult>().Which.StatusCode
                .Should().Be(200);
            result.Result
                .Should().BeOfType<OkObjectResult>().Which.Value
                .Should().BeOfType<ResponseMessage<ShoppingCart>>().Which.Data.UserId
                .Should().Be(BasketData.GetBasketData().UserId);
        }

        // Basket get metod

        [Fact()]
        public async void BasketGetT_Valid()
        {
            //Arrange
            _IBasketRepository.Setup(repo => repo.GetBasket(It.IsAny<string>())).ReturnsAsync(BasketData.GetBasketData());
            var userId = BasketData.GetBasketData().UserId;
            //Act
            BasketController pc = new BasketController(_IIdentityService.Object, _ILogger.Object, _Mapper, _IPublishEndpoint.Object, _IBasketRepository.Object);
            var result = await pc.GetBasket(userId);

            // assert        
            result.Result
                .Should().BeOfType<OkObjectResult>().Which.StatusCode
                .Should().Be(200);
            result.Result
                .Should().BeOfType<OkObjectResult>().Which.Value
                .Should().BeOfType<ResponseMessage<ShoppingCart>>().Which.Data.UserId
                .Should().Be(userId);
        }

        //// Basket delete metod

        [Fact()]
        public async void BasketDeleteTest()
        {
            //Arrange
            _IBasketRepository.Setup(repo => repo.DeleteBasket(It.IsAny<string>()));

            //Act
            BasketController pc = new BasketController(_IIdentityService.Object, _ILogger.Object, _Mapper, _IPublishEndpoint.Object, _IBasketRepository.Object);
            var result = await pc.DeleteBasket(BasketData.GetBasketData().UserId);

            // assert        
            result
                .Should().BeOfType<NoContentResult>().Which.StatusCode
                .Should().Be(204);
        }
    }
}