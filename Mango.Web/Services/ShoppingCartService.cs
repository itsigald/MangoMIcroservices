using Mango.Web.Interfaces;
using Mango.Web.Models;
using System.Runtime;

namespace Mango.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IBaseService _service;
        private readonly IAppSettings _settings;

        public ShoppingCartService(IBaseService service, IAppSettings settings)
        {
            _service = service;
            _settings = settings;
        }

        public async Task<ResponseDto?> GetAllShoppingCartsAsync(string userId)
        {
            return await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.GET,
                Url = $"{_settings.ShoppingCartUrlBase}{_settings.ShoppingCartAPI}/GetCart/{userId}"
            });
        }

        public async Task<ResponseDto?> GetShoppingCartAsync(string userId, int cartId)
        {
            return await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.GET,
                Url = $"{_settings.ShoppingCartUrlBase}{_settings.ShoppingCartAPI}/GetCart/{userId}/{cartId}"
            });
        }

        public async Task<ResponseDto?> UpsertShoppingCartAsync(CartDto cartDto)
        {
            return await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.POST,
                Data = cartDto,
                Url = $"{_settings.ShoppingCartUrlBase}{_settings.ShoppingCartAPI}/CartUpsert"
            });
        }

        public async Task<ResponseDto?> ApplyCouponAsync(ApplyCouponDto applyCouponDto)
        {
            return await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.POST,
                Data = applyCouponDto,
                Url = $"{_settings.ShoppingCartUrlBase}{_settings.ShoppingCartAPI}/ApplyCoupon"
            });
        }

        public async Task<ResponseDto?> RemoveShoppingCartAsync(int cartId)
        {
            return await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.POST,
                Data = cartId,
                Url = $"{_settings.ShoppingCartUrlBase}{_settings.ShoppingCartAPI}/RemoveCart"
            });
        }
    }
}
