using Mango.Services.ShoppingCartAPI.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using System.Text.Json;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<CouponDto?> GetCoupon(string couponCode)
        {
            using (var client = _httpClientFactory.CreateClient("Coupon"))
            {
                var response = await client.GetAsync($"/api/CouponAPI/GetByCode/{couponCode}");

                if (response.IsSuccessStatusCode)
                {
                    var contextString = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(contextString))
                    {
                        ResponseDto? responseDto = JsonSerializer.Deserialize<ResponseDto>(contextString, _jsonSerializerOptions);

                        if (responseDto != null && responseDto.IsSuccess)
                        {
                            return JsonSerializer.Deserialize<CouponDto>(Convert.ToString(responseDto.Result), _jsonSerializerOptions);
                        }
                        else
                        {
                            return new CouponDto();
                        }
                    }
                    else
                    {
                        return new CouponDto();
                    }
                }
                else
                {
                    return new CouponDto();
                }
            }
        }
    }
}
