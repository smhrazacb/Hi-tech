using AutoMapper;
using Basket.API.Entities;
using Basket.API.Entities.Dtos;
using EventBus.Messages.Events;
using EventBus.Messages.Models;

namespace Basket.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDto>()
                .ReverseMap();
            CreateMap<ShoppingItem, ShoppintItemDto>()
                .ReverseMap();
            CreateMap<BasketCheckoutEvent, BasketCheckoutIdsDto>()
                .ReverseMap();
            CreateMap<CatalogStockDelEvent, ShoppingCart>()
             .ReverseMap();  
        }
    }
}
