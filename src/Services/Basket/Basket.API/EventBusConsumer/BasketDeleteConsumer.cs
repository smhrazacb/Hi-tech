﻿using AutoMapper;
using Basket.API.Services.Interfaces;
using EventBus.Messages.Events.Basket;
using MassTransit;

namespace Basket.API.EventBusConsumer
{
    public class BasketDeleteConsumer : IConsumer<BasketDeleteEvent>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<BasketDeleteConsumer> _logger;
        private readonly IBasketRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketDeleteConsumer(IMapper mapper, ILogger<BasketDeleteConsumer> logger, IBasketRepository repository, IPublishEndpoint publishEndpoint)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = repository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<BasketDeleteEvent> context)
        {
            // Get Basket
            var basket = await _repository.GetBasket(context.Message.UserId);
            if (basket == null)
            {
                _logger.LogInformation($"ShoppingCart not found Id : {context.Message.UserId}");
                return;
            }
            // remove the basket
            await _repository.DeleteBasket(basket.UserId);
            _logger.LogInformation($"OrderCompleteEvent consumed successfully. Deleted Shopping Cart Id : {basket.UserId}");
        
        }
    }
}



