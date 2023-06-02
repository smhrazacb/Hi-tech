﻿using AutoMapper;
using EventBus.Messages.Events.Basket;
using EventBus.Messages.Events.Catalog;
using EventBus.Messages.Events.Order;
using EventBus.Messages.Models;
using MassTransit;
using MediatR;
using Ordering.API.Services;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Domain.Entities;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IIdentityService _IdentityService;
        private readonly ILogger<BasketCheckoutConsumer> _logger;

        public BasketCheckoutConsumer(IMediator mediator, IMapper mapper, IPublishEndpoint publishEndpoint, IIdentityService identityService, ILogger<BasketCheckoutConsumer> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _IdentityService = identityService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            CheckoutOrderCommand command;
            try
            {
                command = _mapper.Map<CheckoutOrderCommand>(context.Message);
                command.OrderStatuses = new List<CheckoutOrderCommandOrderStatus>()
                { new CheckoutOrderCommandOrderStatus
                (_IdentityService.GetUserIdentity(), EOrderStatus.Initiated)
                };

                command.OrderId = await _mediator.Send(command);
                _logger.LogInformation($"BasketCheckoutEvent consumed successfully. Created Order Id : {command.OrderId}");

                var orderStatusChangedToIniatedEvent = _mapper.Map<OrderStatusChangedEvent>(command);
                await _publishEndpoint.Publish(orderStatusChangedToIniatedEvent);
                _logger.LogInformation
                    ($"Publishing OrderStatusChangedEvent for " +
                    $"Order Id : {command.OrderId}" +
                    $" Status : {command.OrderStatuses.LastOrDefault().Status}" +
                    $"Date Time : {command.OrderStatuses.LastOrDefault().DateTimeStamp}"
                    );

                var basketDeleteEvent = _mapper.Map<BasketDeleteEvent>(command);
                await _publishEndpoint.Publish(basketDeleteEvent);
                _logger.LogInformation($"Publishing BasketDeleteEvent Event for Order Id : {command.OrderId}");

                var catalogStockDelEvent = _mapper.Map<CatalogStockDelEvent>(command);
                await _publishEndpoint.Publish(catalogStockDelEvent);
                _logger.LogInformation($"Publishing CatalogStockDelEvent for Order Id : {command.OrderId}");
            }
            catch (Exception ex)
            {
                var failedevent = _mapper.Map<OrderStatusChangedEvent>(context.Message);
                _logger.LogError($"UserId Id : {context.Message.UserId} Error {ex.Message}");
                failedevent.OrderStatuses.Add(new EventOrderStatus(
                    _IdentityService.GetUserIdentity(), EventEOrderStatus.Failed, ex.Message)
                );
                await _publishEndpoint.Publish(failedevent);
            }
        }
    }
}
