using AutoMapper;
using Catalog.API.Entities;
using EventBus.Messages.Events.Catalog;

namespace Catalog.API.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CatalogStockUpdatedEvent, CatalogStockDelEvent>().ReverseMap();
        }
    }
}
