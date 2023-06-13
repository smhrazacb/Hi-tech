using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Ordering.API.EventBusConsumer;
using Ordering.API.Services;
using Ordering.Application.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData;

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

        public void ConsumeCheckoutEvent_Sucessfy()
        {
            //Arrange
            _IdentityService
                    .Setup(Object => Object.GetUserIdentity())
                    .Returns(BasketData.GetBasketData().UserId);
            _mediator.Setup(x => x.Send(It.IsAny<OrderData.GetBasketCheckoutConsumerDummyData>())
                  .Returns(Task.FromResult(dataValid));






            var basketCheckoutConsumer = new BasketCheckoutConsumer(_IdentityService.Object, _logger.Object, _publishEndpoint.Object, _mediator.Object,_Mapper);

          
            //Act

            
            //Assert
        }




    }
}
