using MediatR;
using Ordering.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public virtual IEnumerable<UpdateOrderCommandItems> ShoppingItems { get; set; }
        public IList<UpdateOrderCommandOrderStatus> OrderStatuses { get; set; }

        // BillingAddress
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        // Payment
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }
    }
    public class UpdateOrderCommandOrderStatus 
    {
        public string Status { get; set; }
        public DateTime DateTimeStamp { get; private set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; }
        public UpdateOrderCommandOrderStatus(string updatedBy, string status)
        {
            UpdatedBy = updatedBy;
            Status = status;
        }
    }
    public class UpdateOrderCommandItems
    {
        public string ProductId { get; set; }
        public string ProductNameShortdesc { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string? PictureUrl { get; set; }
    }
}
