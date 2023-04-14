using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQuery : IRequest<List<OrdersVm>>
    {
        public Guid UserId { get; set; }

        public GetOrdersListQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
