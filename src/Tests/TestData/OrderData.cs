using Basket.API.Entities;
using EventBus.Messages.Common;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Ordering.Application.Features.Orders.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestData
{
    public class OrderData
    {
        public static IList<OrderQueryModel> Orders(string userName)
        {
            return new List<OrderQueryModel>()
            {
                 new OrderQueryModel()
                {
                    OrderId = 1,
                    UserId = "1",
                    TotalPrice = 100,
                    OrderStatuses = new List<GetOrderStatus>() { },
                    // BillingAddress
                    FirstName = "test",
                    LastName = "test",
                    EmailAddress = "",
                    AddressLine = "",
                    Country = "",
                    State = "",
                    ZipCode = "",
                    // Payment
                    CardName = "",
                    CardNumber = "",
                    Expiration = "",
                    CVV = "",
                    PaymentMethod = 1
                }

            };              

        }
    }
}

