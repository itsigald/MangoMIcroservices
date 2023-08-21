using Mango.Web.Models;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace Mango.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        [Authorize]
        public async Task<IActionResult> ShoppingCartIndex()
        {
            return View(await LoadShoppingCartFromLoggedUser());
        }

        private async Task<CartDto?> LoadShoppingCartFromLoggedUser()
        {
            CartDto? cartDto = null;

            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.UniqueName)?.Value;

            if(!string.IsNullOrEmpty(userId))
            {
                var response = await _shoppingCartService.GetShoppingCartsAsync(userId);

                if(response != null && response.IsSuccess)
                {
                    cartDto = JsonSerializer.Deserialize<CartDto?>(Convert.ToString(response.Result), _jsonSerializerOptions);
                }
            }

            return cartDto;
        }

        public async Task<IActionResult> RemoveItem (int cartDetailsId)
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.UniqueName)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                var response = await _shoppingCartService.RemoveShoppingCartAsync(cartDetailsId);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Shopping Cart update successfully";
                    return RedirectToAction(nameof(ShoppingCartIndex));
                }
            }
            else
            {
                RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.UniqueName)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                ApplyCouponDto applyCouponDto = new ApplyCouponDto
                {
                    UserId = userId,
                    CouponCode = cartDto.CartHeader.CouponCode,
                    CartHeaderId =  cartDto.CartHeader.CartHeaderId
                };

                var response = await _shoppingCartService.ApplyCouponAsync(applyCouponDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = $"Coupon {cartDto.CartHeader.CouponCode} apply to Shopping Cart successfully";
                    return RedirectToAction(nameof(ShoppingCartIndex));
                }
            }
            else
            {
                RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.UniqueName)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                ApplyCouponDto removeCouponDto = new ApplyCouponDto
                {
                    UserId = userId,
                    CouponCode = null,
                    CartHeaderId = cartDto.CartHeader.CartHeaderId
                };

                var response = await _shoppingCartService.ApplyCouponAsync(removeCouponDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = $"Coupon {cartDto.CartHeader.CouponCode} apply to Shopping Cart successfully";
                    return RedirectToAction(nameof(ShoppingCartIndex));
                }
                else
                {
                    TempData["error"] = $"Error on remove coupon: {response?.Message}";
                }
            }
            else
            {
                RedirectToAction("Login", "Auth");
            }

            return View();
        }
    }
}
