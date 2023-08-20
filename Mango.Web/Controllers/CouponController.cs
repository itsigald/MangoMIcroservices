using Mango.Web.Models;
using Mango.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? listCoupon = null;

            ResponseDto? response = await _couponService.GetAllCouponsAsync();
            
            if (response != null && response.IsSuccess)
            {
                if(response.Result != null)
                {
                    listCoupon = JsonSerializer.Deserialize<List<CouponDto>>(Convert.ToString(response.Result), _jsonSerializerOptions);
                }
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            
            return View(listCoupon);
        }
        

        public IActionResult CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponCreateDto coupon)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? responseDto = await _couponService.CreateCouponAsync(coupon);

                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = $"Coupon create with successfully.";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = responseDto?.Message;
                }
            }

            return View(coupon);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
            ResponseDto? responseDto = await _couponService.GetCouponByIdAsync(couponId);

            if (responseDto != null && responseDto.IsSuccess)
            {
                CouponDto coupon = JsonSerializer.Deserialize<CouponDto>(Convert.ToString(responseDto.Result), _jsonSerializerOptions);
                return View(coupon);
            }
            else
            {
                TempData["error"] = responseDto?.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto coupon)
        {
            ResponseDto? responseDto = await _couponService.DeleteCouponAsync(coupon.CouponId);

            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = $"Coupon delete with successfully.";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = responseDto?.Message;
            }

            return View(coupon);
        }

        public async Task<IActionResult> CouponUpdate(int couponId)
        {
            ResponseDto? responseDto = await _couponService.GetCouponByIdAsync(couponId);

            if (responseDto != null && responseDto.IsSuccess)
            {
                CouponDto coupon = JsonSerializer.Deserialize<CouponDto>(Convert.ToString(responseDto.Result), _jsonSerializerOptions);
                return View(coupon);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponUpdate(CouponDto coupon)
        {
            ResponseDto? responseDto = await _couponService.UpdateCouponAsync(coupon);

            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = $"Coupon update with successfully.";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = responseDto?.Message;
            }

            return View(coupon);
        }
    }
}
