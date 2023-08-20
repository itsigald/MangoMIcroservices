using Mango.Services.ShoppingCartAPI.Dtos;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponCode);
    }
}
