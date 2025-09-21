using AutoMapper;
using SCD.Services.OrderAPI.Models;
using SCD.Services.OrderAPI.Models.Dto;

namespace SCD.Services.OrderAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                // OrderHeader ↔ OrderHeaderDto
                config.CreateMap<OrderHeader, OrderHeaderDto>()
                      .ReverseMap();

                // If needed, map OrderDetails too
                config.CreateMap<OrderDetails, OrderDetailsDto>()
                      .ReverseMap();

                // You already had this
                config.CreateMap<OrderHeaderDto, CartHeaderDto>()
                      .ReverseMap();
            });
            return mappingConfig;
        }
    }
}
