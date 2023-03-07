using AutoMapper;
using Customer.API.Entities;
using Customer.API.Entities.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Customer.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ReverseMap();
            CreateMap<Address, AddressDto>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ReverseMap();
            CreateMap<GeoData, GeoDataDto>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ReverseMap();
            CreateMap<UserDto, IdentityUser>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ReverseMap();
        }
    }
}
