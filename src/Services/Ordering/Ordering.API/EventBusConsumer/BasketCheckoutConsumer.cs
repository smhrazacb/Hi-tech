using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
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
            var orderCompleteEvent = _mapper.Map<OrderCompleteEvent>(command);

            _logger.LogInformation($"BasketCheckoutEvent consumed successfully. Created Order Id : {command.OrderId}");
            // send delete basket event to rabbitmq
            await _publishEndpoint.Publish(orderCompleteEvent);
        }
    }
}
