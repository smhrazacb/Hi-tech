﻿using MediatR;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommand : IRequest<int>
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public  IEnumerable<CheckoutOrderCommandItems> ShoppingItems { get; set; }
        public IList<CheckoutOrderCommandOrderStatus> OrderStatuses { get; set; }

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
    public class CheckoutOrderCommandOrderStatus
    {
        public string Status { get; set; }
        public DateTimeOffset DateTimeStamp { get; } = DateTimeOffset.UtcNow;

        public CheckoutOrderCommandOrderStatus(string status)
        {
            Status = status;
        }
    }
    public class CheckoutOrderCommandItems
    {
        public string ProductId { get; set; }
        public string ProductNameShortdesc { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string? PictureUrl { get; set; }
    }
}
