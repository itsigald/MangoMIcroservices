using AutoMapper;
using Mango.Services.CouponAPI.Dtos;
using Mango.Services.CouponAPI.Models;

namespace Mango.Services.CouponAPI
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
                config.CreateMap<CouponDto, Coupon>();
                config.CreateMap<Coupon, CouponDto>();
                config.CreateMap<CouponCreateDto, Coupon>();
            });

            return mappingConfig;
        }
    }
}
