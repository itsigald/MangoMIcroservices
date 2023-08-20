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

        private async Task<List<CartDto>?> LoadShoppingCartFromLoggedUser()
        {
            List<CartDto>? listCartDto = null;

            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.UniqueName)?.Value;

            if(!string.IsNullOrEmpty(userId))
            {
                var response = await _shoppingCartService.GetAllShoppingCartsAsync(userId);

                if(response != null && response.IsSuccess)
                {
                    listCartDto = JsonSerializer.Deserialize<List<CartDto>>(Convert.ToString(response.Result), _jsonSerializerOptions);
                }
            }

            return listCartDto;
        }
    }
}
