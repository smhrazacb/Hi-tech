using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrder, OrderQueryModel>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrderQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OrderQueryModel> Handle(GetOrder request, CancellationToken cancellationToken)
        {
            var orderList = await _orderRepository.GetOrdersByOrderId(request.OrderId);
            return _mapper.Map<OrderQueryModel>(orderList);
        }
    }
}
