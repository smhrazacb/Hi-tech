using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQuery : IRequest<List<OrderQueryModel>>
    {
        public string UserId { get; set; }

        public GetOrdersListQuery(string userId)
        {
            UserId = userId;
        }
    }
}
