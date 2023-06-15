//using AutoMapper;
//using EventBus.Messages.Events.Basket;
//using EventBus.Messages.Events.Order;
//using EventBus.Messages.Models;
//using MassTransit;
//using MassTransit.Transports;
//using MediatR;
//using Microsoft.Extensions.Logging;
//using Moq;
//using Ordering.API.EventBusConsumer;
//using Ordering.API.Services;
//using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
//using Ordering.Application.Mappings;
//using Ordering.Domain.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TestData;
//using static StackExchange.Redis.Role;

//namespace Order.APITests.EventBusConsumer
//{
//    public class BasketCheckoutConsumerTests
//    {
//        private readonly Mock<IMediator> _mediator;
//        private readonly AutoMapper.Mapper _Mapper;
//        private readonly Mock<IPublishEndpoint> _publishEndpoint;
//        private readonly Mock<IIdentityService> _IdentityService;
//        private readonly Mock<ILogger<BasketCheckoutConsumer>> _logger;

//        public BasketCheckoutConsumerTests()
//        {
//            _mediator = new Mock<IMediator>();
//            _publishEndpoint = new Mock<IPublishEndpoint>();
//            _IdentityService = new Mock<IIdentityService>();
//            _logger = new Mock<ILogger<BasketCheckoutConsumer>>();
//            var myProfile = new MappingProfile();
//            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
//            _Mapper = new AutoMapper.Mapper(configuration);
//        }

//        [Fact]
//        public void ConsumeCheckoutEvent_Sucessfy()
//        {
//            //Arrange
//            _IdentityService
//                .Setup(Object => Object.GetUserIdentity())
//                .Returns("testId");

//            var context = new Mock<ConsumeContext<BasketCheckoutEvent>>();

//            var input = new BasketCheckoutEvent()
//            {
//                UserId = "testId",
//                AddressLine = "Test",
//                CardName = "Test",
//                CardNumber = "Test",
//                Country = "Test",
//                CVV = "Test",
//                EmailAddress = "Test",
//                Expiration = "Test",
//                FirstName = "Test",
//                LastName = "Test",
//                PaymentMethod = 1,
//                State = "Test",
//                TotalPrice = 100,
//                ZipCode = "Test",

//                ShoppingItems = new List<EventCartItem>()
//                {
//                     new EventCartItem()
//                    {
//                        ProductId = "11",
//                        PictureUrl = "11",
//                        ProductNameShortdesc = "11",
//                        Quantity = 11,
//                        UnitPrice = 11
//                    },
//                    new EventCartItem()
//                    {
//                        ProductId = "22",
//                        PictureUrl = "22",
//                        ProductNameShortdesc = "22",
//                        Quantity = 22,
//                        UnitPrice = 22
//                    }
//                },
//            };

//            context.Setup(c => c.Message).Returns(input);
            
//            _mediator
//                    .Setup(x => x.Send(It.IsAny<CheckoutOrderCommand>(), default))
//                    .ReturnsAsync(OrderData.GetBasketCheckoutConsumerDummyData().OrderId);



//            //Act
//            var basketCheckoutConsumer = new BasketCheckoutConsumer(_mediator.Object, _Mapper, _publishEndpoint.Object, _IdentityService.Object, _logger.Object);

//            var result = basketCheckoutConsumer.Consume(context.Object);


//            //Assert

//            // Assert that the order was processed successfully
//            _logger.Verify(
//                x => x.LogInformation($"BasketCheckoutEvent consumed successfully. Created Order Id : {_mediator.Object}"),
//                Times.Once
//            );

//            // Assert that the OrderStatusChangedEvent was published
//            _publishEndpoint.Verify(
//                x => x.Publish(It.IsAny<OrderStatusChangedEvent>(), default),
//                Times.Once
//            );

//            // Assert that the BasketDeleteEvent was published
//            _publishEndpoint.Verify(
//                x => x.Publish(It.IsAny<BasketDeleteEvent>(), default),
//                Times.Once
//            );

//            _publishEndpoint.Verify(
//                x => x.Publish(It.IsAny<OrderStatusChangedEvent>(), default),
//                Times.Once
//            );

//            //_Mapper.Verify(m => m.Map<OrderStatusChangedEvent>(checkoutOrderCommand), Times.Once);
//            //_mapperMock.Verify(m => m.Map<BasketDeleteEvent>(checkoutOrderCommand), Times.Once);
//            //_mapperMock.Verify(m => m.Map<CatalogStockDelEvent>(checkoutOrderCommand), Times.Once);
//            //_publishEndpointMock.Verify(p => p.Publish(orderStatusChangedEvent), Times.Once);
//            //_publishEndpointMock.Verify(p => p.Publish(basketDeleteEvent), Times.Once);
//            //_publishEndpointMock.Verify(p => p.Publish(catalogStockDelEvent), Times.Once);
//            //_loggerMock.Verify(l => l.LogInformation($"BasketCheckoutEvent consumed successfully. Created Order Id : {orderId}"), Times.Once);
//            //_loggerMock.Verify(l => l.LogInformation(
//            //    $"Publishing OrderStatusChangedEvent for Order Id : {orderId} Status : {orderStatusChangedEvent.OrderStatuses.LastOrDefault().Status} Date Time : {orderStatusChangedEvent.OrderStatuses.LastOrDefault().DateTimeStamp}"
//            //    ), Times.Once);
//            //_loggerMock.Verify(l => l.LogInformation($"Publishing BasketDeleteEvent Event for Order Id : {orderId}"), Times.Once);
//            //_loggerMock.Verify(l => l.LogInformation($"Publishing CatalogStockDelEvent for Order Id : {orderId}"), Times.Once);
//        }




//    }
//}
