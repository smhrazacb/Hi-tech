using AutoMapper;
using Basket.API.Repositories.Interfaces;
using EventBus.Messages.Events;
using EventBus.Messages.Models;
using MassTransit;
using MassTransit.Transports;

namespace Basket.API.EventBusConsumer
{
    public class BasketDeleteConsumer : IConsumer<OrderCompleteEvent>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<BasketDeleteConsumer> _logger;
        private readonly IBasketRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketDeleteConsumer(IMapper mapper, ILogger<BasketDeleteConsumer> logger, IBasketRepository repository)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<OrderCompleteEvent> context)
        {
            // Get Basket
            var basket = await _repository.GetBasket(context.Message.ShoppingCartId);
            if (basket == null)
            {
                _logger.LogInformation($"ShoppingCart not found Id : {context.Message.ShoppingCartId}");
                return;
            }
            // remove the basket
            await _repository.DeleteBasket(basket.ShoppingCartId);
            _logger.LogInformation($"OrderCompleteEvent consumed successfully. Deleted Shopping Cart Id : {basket.ShoppingCartId}");
            
            var catalogStockDelEvent = _mapper.Map<CatalogStockDelEvent>(basket.ShoppingItems);
            await _publishEndpoint.Publish(catalogStockDelEvent);
            _logger.LogInformation($"Publishing CatalogStockDelEvent for Order Id : {context.Message.OrderId}");
        }
    }
}



