using AutoMapper;
using EventBus.Messages.Events.Catalog;
using EventBus.Messages.Events.Order;
using EventBus.Messages.Models;
using MassTransit;
using MediatR;
using Ordering.API.Services;
using Ordering.Application.Features.Orders.Commands.UpdateOrderStatus;
using Ordering.Domain.Entities;

namespace Ordering.API.EventBusConsumer
{
    public class CatalogStockUpdatedConsumer : IConsumer<CatalogStockUpdatedEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IIdentityService _IdentityService;
        private readonly ILogger<CatalogStockUpdatedConsumer> _logger;

        public CatalogStockUpdatedConsumer(IMediator mediator, IMapper mapper, IPublishEndpoint publishEndpoint, IIdentityService identityService, ILogger<CatalogStockUpdatedConsumer> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _IdentityService = identityService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CatalogStockUpdatedEvent> context)
        {
            try
            {

            var command = _mapper.Map<UpdateOrderStatusCommand>(context.Message);
            command.OrderStatus = new(EventEOrderStatus.Confirmed.ToString());
            var updatedOrder = await _mediator.Send(command);


            _logger.LogInformation($"CatalogStockUpdatedEvent consumed successfully. " +
                $"Updated Order Id : {command.OrderId} Status : {updatedOrder.OrderStatuses.LastOrDefault().Status}");


            var orderStatusChangedToIniatedEvent = _mapper.Map<OrderStatusChangedEvent>(updatedOrder);
            await _publishEndpoint.Publish(orderStatusChangedToIniatedEvent);
            _logger.LogInformation
                ($"Publishing OrderStatusChangedEvent for " +
                $"Order Id : {orderStatusChangedToIniatedEvent.OrderId}" +
                $" Status : {orderStatusChangedToIniatedEvent.OrderStatuses.LastOrDefault().Status}" +
                $"Date Time : {orderStatusChangedToIniatedEvent.OrderStatuses.LastOrDefault().DateTimeStamp}"
                );
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
