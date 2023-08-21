using AutoMapper;
using Mango.Services.ProductAPI.Dtos;
using Mango.Services.ProductAPI.Models;

namespace Mango.Services.ProductAPI
{
    public class MappingConfig
    {
        public MappingConfig()
        {
            
        }

        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>().ReverseMap();
                config.CreateMap<ProductCreateDto, Product>();
            });

            return mappingConfig;
        }
    }
}
