﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            Order order;
            var orderEntity = _mapper.Map<Order>(request);
            //check duplicate 
            var duplicateOrder = await _orderRepository.GetOrdersByUserId(orderEntity.UserId);
            if (duplicateOrder.Count() != 0)
            {
                order = duplicateOrder.FirstOrDefault();
                _logger.
                    LogInformation($"Shopping Cart Id : {order.UserId} " +
                    $"is already assioted with Order Id .{order.OrderId}");
            }
            else
            {
                //create new order 
                order = await _orderRepository.AddAsync(orderEntity);
                _logger.LogInformation($"Order {order.OrderId} is successfully created.");
                //await SendMail(order);
            }

            return order.OrderId;
        }

        private async Task SendMail(Order order)
        {
            var email = new Email() { To = "ezozkme@gmail.com", Body = $"Order was created.", Subject = "Order was created" };

            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Order {order.OrderId} failed due to an error with the mail service: {ex.Message}");
            }
        }
    }
}
