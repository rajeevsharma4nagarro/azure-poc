using AutoMapper;
using SCD.Services.CartAPI.Models;
using SCD.Services.CartAPI.Models.Dto;

namespace SCD.Services.CartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsResponseDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
