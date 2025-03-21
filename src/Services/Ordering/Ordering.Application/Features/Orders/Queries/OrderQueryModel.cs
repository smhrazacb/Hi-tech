﻿using Newtonsoft.Json.Converters;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Domain.Entities;
using System.Text.Json.Serialization;

namespace Ordering.Application.Features.Orders.Queries
{
    public class OrderQueryModel
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public IEnumerable<GetOrderItem> ShoppingItems { get; set; }
        public IList<GetOrderStatus> OrderStatuses { get; set; }

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
    public class GetOrderStatus
    {
        public string Status { get; set; }
        public DateTimeOffset DateTimeStamp { get; private set; } = DateTimeOffset.UtcNow;
        public string UpdatedBy { get; set; }
    }
    public class GetOrderItem
    {
        public string ProductId { get; set; }
        public string ProductNameShortdesc { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal OldUnitPrice { get; set; }
        public int Quantity { get; set; }
        public string? PictureUrl { get; set; }
    }
}
