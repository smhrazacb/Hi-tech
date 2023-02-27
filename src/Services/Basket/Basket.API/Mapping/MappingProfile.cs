using AutoMapper;
using Basket.API.Entities;
using Basket.API.Entities.Dtos;

namespace Customer.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDto>()
                .ReverseMap();
            CreateMap<ShoppingItem, ShoppintItemDto>()
                .ReverseMap();
        }
    }
}
