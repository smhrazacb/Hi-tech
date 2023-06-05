using AutoMapper;
using EventBus.Messages.Events.Basket;
using EventBus.Messages.Events.Catalog;
using EventBus.Messages.Events.Order;
using EventBus.Messages.Models;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrderStatus;
using Ordering.Domain.Entities;

namespace Ordering.API.Mapper
{
    public class OrderingProfile : Profile
	{
		public OrderingProfile()
		{
			CreateMap<CheckoutOrderCommand, BasketCheckoutEvent>().ReverseMap();
			CreateMap<CheckoutOrderCommandItems, EventCartItem>().ReverseMap();
			CreateMap<CheckoutOrderCommand, BasketDeleteEvent>().ReverseMap();
			CreateMap<CheckoutOrderCommand, CatalogStockDelEvent>().ReverseMap();
			CreateMap<CheckoutOrderCommand, OrderStatusChangedEvent>().ReverseMap();
			CreateMap<CheckoutOrderCommandOrderStatus, EventOrderStatus>().ReverseMap();
			CreateMap<Order, OrderStatusChangedEvent>().ReverseMap();
			CreateMap<OrderStatus, EventOrderStatus>().ReverseMap();
			CreateMap<UpdateOrderStatusCommand, CatalogStockUpdatedEvent>().ReverseMap();
		}
	}
}
