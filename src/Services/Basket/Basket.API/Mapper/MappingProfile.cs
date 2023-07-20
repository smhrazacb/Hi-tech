using AutoMapper;
using Basket.API.Entities;
using Basket.API.Entities.Dtos;
using EventBus.Messages.Events.Basket;
using EventBus.Messages.Events.Catalog;
using EventBus.Messages.Models;

namespace Basket.API.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDto>()
                .ReverseMap();
            CreateMap<ShoppingItem, ShoppintItemDto>()
                .ReverseMap();  
            CreateMap<ShoppingItem, EventCartItem>()
                .ReverseMap();
            CreateMap<BasketCheckoutEvent, BasketCheckoutIdsDto>()
                .ReverseMap();
            CreateMap<ShoppingCart, CatalogStockDelEvent>()
               .ReverseMap();
        }
    }
}
