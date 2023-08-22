using Mango.MessageBus;
using Mango.Web.Interfaces;
using Mango.Web.Models;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Mango.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IMessageBus _messageBus;
        private readonly IAppSettings _appSettings;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly ResponseDto _response;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IAppSettings appSettings, IMessageBus messageBus)
        {
            _shoppingCartService = shoppingCartService;
            _messageBus = messageBus;
            _appSettings = appSettings;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _response = new ResponseDto();
        }

        [Authorize]
        public async Task<IActionResult> ShoppingCartIndex()
        {
            return View(await LoadShoppingCartFromLoggedUser(null));
        }

        private async Task<CartDto?> LoadShoppingCartFromLoggedUser(int? cartId = null)
        {
            CartDto? cartDto = null;

            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.UniqueName)?.Value;

            if(!string.IsNullOrEmpty(userId))
            {
                ResponseDto? response;

                if(cartId == null)
                {
                    response = await _shoppingCartService.GetShoppingCartsAsync(userId);
                }
                else
                {
                    response = await _shoppingCartService.GetShoppingCartAsync(userId, cartId.Value);
                }

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

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.UniqueName)?.Value;
            var emailTo = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)?.Value;
            var fullname = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                var cart = await LoadShoppingCartFromLoggedUser(cartDto.CartHeader.CartHeaderId);

                cart.EmailInfoDto = new EmailInfoDto
                {
                    EmailFrom = _appSettings.EmailFrom,
                    EmailTo = emailTo,
                    FullNane = fullname
                };

                var response = await _shoppingCartService.EmailCartAsync(cart);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = $"Email will send in few secondos...";
                    return RedirectToAction(nameof(ShoppingCartIndex));
                }
                else
                {
                    TempData["error"] = $"Error Send Email: {response?.Message}";
                }
            }
            else
            {
                RedirectToAction("Login", "Auth");
            }

            return RedirectToAction(nameof(ShoppingCartIndex));
        }
    }
}
