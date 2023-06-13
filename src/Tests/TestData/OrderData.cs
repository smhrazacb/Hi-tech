using Basket.API.Entities;
using Basket.API.Entities.Dtos;
using EventBus.Messages.Common;
using EventBus.Messages.Events.Basket;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Ordering.API.EventBusConsumer;
using Ordering.Application.Features.Orders.Queries;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;


namespace TestData
{
    public static class OrderData
    {
       public static List<OrderQueryModel> Orders()
        {
            return new List<OrderQueryModel>()
            {
                new OrderQueryModel() {
                OrderId = 1,
                UserId = "Test",

                TotalPrice = 1000,
                FirstName = "Test",
                LastName = "Test",
                EmailAddress = "Test@test.com",
                AddressLine = "Test",
                Country = "Test",
                State = "Test",
                ZipCode = "Test",
                CardName = "Test",
                CardNumber = "Test",
                Expiration = "Test",
                CVV = "Test",
                PaymentMethod = 1,
                ShoppingItems = new List<GetOrderItem>()
                {
                     new GetOrderItem()
                    {
                        ProductId = "11",
                        PictureUrl = "11",
                        ProductNameShortdesc = "11",
                        Quantity = 11,
                        UnitPrice = 11
                    },
                    new GetOrderItem()
                    {
                        ProductId = "22",
                        PictureUrl = "22",
                        ProductNameShortdesc = "22",
                        Quantity = 22,
                        UnitPrice = 22
                    }
                },
                OrderStatuses = new List<GetOrderStatus>()
                {
                    new GetOrderStatus()
                    {
                        Status = EOrderStatus.Initiated,
                    }

                }

                },
                new OrderQueryModel() {
                OrderId = 1,
                UserId = "Test1",

                TotalPrice = 1000,
                FirstName = "Test",
                LastName = "Test",
                EmailAddress = "Test@test.com",
                AddressLine = "Test",
                Country = "Test",
                State = "Test",
                ZipCode = "Test",
                CardName = "Test",
                CardNumber = "Test",
                Expiration = "Test",
                CVV = "Test",
                PaymentMethod = 1,
                ShoppingItems = new List<GetOrderItem>()
                {
                     new GetOrderItem()
                    {
                        ProductId = "11",
                        PictureUrl = "11",
                        ProductNameShortdesc = "11",
                        Quantity = 11,
                        UnitPrice = 11
                    },
                    new GetOrderItem()
                    {
                        ProductId = "22",
                        PictureUrl = "22",
                        ProductNameShortdesc = "22",
                        Quantity = 22,
                        UnitPrice = 22
                    }
                },
                OrderStatuses = new List<GetOrderStatus>()
                {
                    new GetOrderStatus()
                    {
                        Status = EOrderStatus.Initiated,
                    }

                }

                }
            };
        }

        public static BasketCheckoutEvent GetBasketCheckoutConsumerDummyData(string userId)
        {
            return new BasketCheckoutEvent()
            {
                UserId = userId,
                AddressLine = "Test",
                CardName = "Test",
                CardNumber = "Test",
                Country = "Test",
                CVV = "Test",
                EmailAddress = "Test",
                Expiration = "Test",
                FirstName = "Test",
                LastName = "Test",
                PaymentMethod = 1,
                State = "Test",
                TotalPrice = 100,
                ZipCode = "Test"
            };


        }

}

