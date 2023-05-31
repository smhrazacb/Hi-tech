using AutoMapper;
using EventBus.Messages.Events;
using EventBus.Messages.Models;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

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
			CreateMap<CheckoutOrderCommand, OrderStatusChangeEvent>().ReverseMap();
		}
	}
}
