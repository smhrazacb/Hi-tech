﻿using AutoMapper;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrderStatus;
using Ordering.Application.Features.Orders.Queries;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderQueryModel>().ReverseMap();
            CreateMap<OrderItem, GetOrderItem>().ReverseMap();
            CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
            CreateMap<OrderItem, CheckoutOrderCommandItems>().ReverseMap();
            CreateMap<Order, UpdateOrderCommand>().ReverseMap();
            CreateMap<OrderItem, UpdateOrderCommandItems>().ReverseMap();
            CreateMap<OrderItem, GetOrderItem>().ReverseMap();
            CreateMap<OrderStatus, UpdateOrderCommandOrderStatus>().ReverseMap();
            CreateMap<OrderStatus, GetOrderStatus>().ReverseMap();
            CreateMap<OrderStatus, CheckoutOrderCommandOrderStatus>().ReverseMap();
            CreateMap<OrderStatus, UpdateOrderStatusCommand>().ReverseMap();
            CreateMap<OrderStatus, UpdateOrderStatusCommandOrderStatus>().ReverseMap();
        }
    }
}
