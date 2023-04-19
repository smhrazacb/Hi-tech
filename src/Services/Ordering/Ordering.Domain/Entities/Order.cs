﻿using Ordering.Domain.Common;

namespace Ordering.Domain.Entities
{
    public class Order : EntityBase
    {
        public decimal TotalPrice { get; set; }
        public virtual IEnumerable<OrderItem> ShoppingItems { get; set; }
        public Guid ShoppingCartId { get; set; }

        // BillingAddress
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        // Payment
        public string CardName
        {
            get;
            set;
        }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }
    }
}
