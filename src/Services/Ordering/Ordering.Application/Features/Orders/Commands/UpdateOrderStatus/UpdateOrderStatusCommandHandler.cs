using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Application.Features.Orders.Commands.UpdateOrderStatus;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{

    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Order>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateOrderStatusCommandHandler> _logger;

        public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<UpdateOrderStatusCommandHandler> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Order> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetByIdAsync(request.OrderId);
            if (orderToUpdate == null)
            {
                throw new NotFoundException(nameof(Order), request.OrderId);
            }
            var ors = _mapper.Map<OrderStatus>(request.OrderStatus);
            orderToUpdate.OrderStatuses.Add(ors);
            await _orderRepository.UpdateAsync(orderToUpdate);

            _logger.LogInformation($"Order {orderToUpdate.OrderId} is successfully updated.");

            return await _orderRepository.GetByIdAsync(request.OrderId);
        }
    }
}
