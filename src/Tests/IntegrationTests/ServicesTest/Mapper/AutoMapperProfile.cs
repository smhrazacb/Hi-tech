using AutoMapper;
using EventBus.Messages.Events;
using EventBus.Messages.Models;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries;

namespace ServicesTest.Mapper
{
    public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
            CreateMap<GetOrderItem, UpdateOrderCommandItems>().ReverseMap();
            CreateMap<GetOrderStatus, UpdateOrderCommandOrderStatus>().ReverseMap();
            CreateMap<OrderQueryModel, UpdateOrderCommand>().ReverseMap();
        }
	}
}
