using AutoMapper;
using EventBus.Messages.Events.Basket;
using EventBus.Messages.Events.Order;
using EventBus.Messages.Models;
using MassTransit;
using MassTransit.Transports;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Ordering.API.EventBusConsumer;
using Ordering.API.Services;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Mappings;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData;
using static StackExchange.Redis.Role;

namespace Order.APITests.EventBusConsumer
{
    public class BasketCheckoutConsumerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly AutoMapper.Mapper _Mapper;
        private readonly Mock<IPublishEndpoint> _publishEndpoint;
        private readonly Mock<IIdentityService> _IdentityService;
        private readonly Mock<ILogger<BasketCheckoutConsumer>> _logger;

        public BasketCheckoutConsumerTests()
        {
            _mediator = new Mock<IMediator>();
            _publishEndpoint = new Mock<IPublishEndpoint>();
            _IdentityService = new Mock<IIdentityService>();
            _logger = new Mock<ILogger<BasketCheckoutConsumer>>();
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _Mapper = new AutoMapper.Mapper(configuration);
        }

        [Fact]
        public async void ConsumeCheckoutEvent_Sucessfy()
        {
            //Arrange
            _IdentityService
                .Setup(Object => Object.GetUserIdentity())
                .Returns("testId");
          
            _mediator
                 .Setup(x => x.Send(It.IsAny<CheckoutOrderCommand>(), default))
                .ReturnsAsync(1);

            var context = new Mock<ConsumeContext<BasketCheckoutEvent>>();

            var input = new BasketCheckoutEvent()
            {
                UserId = "testId",
                AddressLine = "Test",
                CardName = "Test",
                CardNumber = "Test",
                Country = "Test",
                CVV = "Test",
                EmailAddress = "Test",
                Expiration = "Test",
                FirstName = "Test",
                LastName = "Test",
                PaymentMethod = 1,
                State = "Test",
                TotalPrice = 100,
                ZipCode = "Test",

                ShoppingItems = new List<EventCartItem>()
                {
                     new EventCartItem()
                    {
                        ProductId = "11",
                        PictureUrl = "11",
                        ProductNameShortdesc = "11",
                        Quantity = 11,
                        UnitPrice = 11
                    },
                    new EventCartItem()
                    {
                        ProductId = "22",
                        PictureUrl = "22",
                        ProductNameShortdesc = "22",
                        Quantity = 22,
                        UnitPrice = 22
                    }
                },
            };

            var checkOutOrderCommand = new CheckoutOrderCommand()
            {
                OrderId = 1,
            };

            checkOutOrderCommand.OrderStatuses = new List<CheckoutOrderCommandOrderStatus>()
                {
                    new CheckoutOrderCommandOrderStatus
                (_IdentityService.Object.GetUserIdentity(),EventEOrderStatus.Initiated.ToString())
                };

            //checkOutOrderCommand.OrderId = await _mediator.Send(_mediator.Object);


            context.Setup(c => c.Message).Returns(input);





            //Act
            var basketCheckoutConsumer = new BasketCheckoutConsumer(_mediator.Object, _Mapper, _publishEndpoint.Object, _IdentityService.Object, _logger.Object);

            var result =  basketCheckoutConsumer.Consume(context.Object);

            var logMessage = result.Status.ToString();


            //Assert
          
        }




    }
}
