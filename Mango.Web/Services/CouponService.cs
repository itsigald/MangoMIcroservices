using Mango.Web.Interfaces;
using Mango.Web.Models;
using System.Runtime;

namespace Mango.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _service;
        private readonly IAppSettings _settings;

        public CouponService(IBaseService service, IAppSettings settings)
        {
            _service = service;
            _settings = settings;
        }

        public async Task<ResponseDto?> GetAllCouponsAsync()
        {
            var getAll = await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.GET,
                Url = $"{_settings.CouponUrlBase}{_settings.CouponAPI}"
            });

            return getAll;
        }
        public async Task<ResponseDto?> GetCouponByIdAsync(int couponId)
        {
            return await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.GET,
                Url = $"{_settings.CouponUrlBase}{_settings.CouponAPI}/{couponId}"
            });
        }

        public async Task<ResponseDto?> GetCouponAsync(string couponCode)
        {
            return await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.GET,
                Url = $"{_settings.CouponUrlBase}{_settings.CouponAPI}/GetByCode/{couponCode}"
            });
        }

        public async Task<ResponseDto?> CreateCouponAsync(CouponCreateDto coupon)
        {
            return await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.POST,
                Data = coupon,
                Url = $"{_settings.CouponUrlBase}{_settings.CouponAPI}"
            });
        }
        public async Task<ResponseDto?> UpdateCouponAsync(CouponDto coupon)
        {
            return await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.PUT,
                Data = coupon,
                Url = $"{_settings.CouponUrlBase}{_settings.CouponAPI}"
            });
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int couponId)
        {
            return await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.DELETE,
                Url = $"{_settings.CouponUrlBase}{_settings.CouponAPI}/{couponId}"
            });
        }
    }
}
