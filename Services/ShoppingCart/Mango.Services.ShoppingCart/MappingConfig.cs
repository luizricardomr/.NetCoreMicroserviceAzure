using AutoMapper;
using Mango.Services.ShoppingCart.Models;
using Mango.Services.ShoppingCart.Models.DTO;

namespace Mango.Services.ShoppingCart
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeader, CartHeaderDTO>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsDTO>().ReverseMap();
            });

            return mappingConfig;
        } 
    }
}
