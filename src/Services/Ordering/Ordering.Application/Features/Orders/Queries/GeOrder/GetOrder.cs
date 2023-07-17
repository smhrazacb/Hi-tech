using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrder : IRequest<OrderQueryModel>
    {
        public int OrderId { get; set; }

        public GetOrder(int orderId)
        {
            OrderId = orderId;
        }
    }
}
