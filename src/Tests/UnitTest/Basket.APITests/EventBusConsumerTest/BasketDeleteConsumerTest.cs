using AutoMapper;
using Basket.API.Controllers;
using Basket.API.EventBusConsumer;
using Basket.API.Mapper;
using Basket.API.Services;
using Basket.API.Services.Interfaces;
using MassTransit;
using MassTransit.Transports;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.APITests.EventBusConsumerTest
{
    public class BasketDeleteConsumerTest
    {
        private readonly AutoMapper.Mapper _Mapper;
        private readonly Mock<ILogger<BasketDeleteConsumer>> _ILogger;
        private readonly Mock<IBasketRepository> _IBasketRepository;
        private readonly Mock<IPublishEndpoint> _IPublishEndpoint;

        public BasketDeleteConsumerTest()
        {

            _ILogger = new Mock<ILogger<BasketDeleteConsumer>>();
            _IPublishEndpoint = new Mock<IPublishEndpoint>();
            _IBasketRepository = new Mock<IBasketRepository>();
            var myProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _Mapper = new Mapper(configuration);
        }


    }
}
