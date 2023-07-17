using MediatR;
using Ordering.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommand : IRequest<Order>
    {
        public int OrderId { get; set; }
        public UpdateOrderStatusCommandOrderStatus? OrderStatus { get; set; }
    }
    public class UpdateOrderStatusCommandOrderStatus
    {
        public string Status { get; set; }
        public DateTime DateTimeStamp { get; private set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; }
        public UpdateOrderStatusCommandOrderStatus(string updatedBy, string status)
        {
            UpdatedBy = updatedBy;
            Status = status;
        }
    }
}
