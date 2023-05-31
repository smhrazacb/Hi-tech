using AutoMapper;
using EventBus.Messages.Events;
using EventBus.Messages.Models;
using MassTransit;
using MediatR;
using Ordering.API.Services;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IIdentityService _IdentityService;
        private readonly ILogger<BasketCheckoutConsumer> _logger;

        public BasketCheckoutConsumer(IMediator mediator, IMapper mapper, IPublishEndpoint publishEndpoint, ILogger<BasketCheckoutConsumer> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
            command.OrderId = await _mediator.Send(command);
            _logger.LogInformation($"BasketCheckoutEvent consumed successfully. Created Order Id : {command.OrderId}");

            var basketDeleteEvent = _mapper.Map<BasketDeleteEvent>(command);
            await _publishEndpoint.Publish(basketDeleteEvent);
            _logger.LogInformation($"Publishing BasketDeleteEvent Event for Order Id : {command.OrderId}");

            var catalogStockDelEvent = _mapper.Map<CatalogStockDelEvent>(command);
            await _publishEndpoint.Publish(catalogStockDelEvent);
            _logger.LogInformation($"Publishing CatalogStockDelEvent for Order Id : {command.OrderId}");

            //var orderStatusChangeEvent = _mapper.Map<OrderStatusChangeEvent>(command);
            //orderStatusChangeEvent.Statuses.Add(new EventOrderStatus(EOrderStatus.Initiated, _IdentityService.GetUserName()));
            //await _publishEndpoint.Publish(orderStatusChangeEvent);
            //_logger.LogInformation($"Publishing OrderStatusChangeEvent for Order Id : {command.OrderId}");
        }
    }
}
