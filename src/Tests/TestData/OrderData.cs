using Basket.API.Entities;
using EventBus.Messages.Common;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Ordering.Application.Features.Orders.Queries;
using Ordering.Domain.Entities;
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
        public int OrderId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public List<OrderItem> OrderItems { get; set; }
        
    }

    public class OrderItem
    {
        public int ProductId { get; set;}
        public int Quantity { get; set; } 
    }
}

