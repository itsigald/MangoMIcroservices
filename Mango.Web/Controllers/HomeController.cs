using Mango.Web.Models;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IShoppingCartService _cartService;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public HomeController(ILogger<HomeController> logger, IProductService productService, IShoppingCartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<ProductDto>? listProduct = null;

            ResponseDto? response = await _productService.GetAllProductsAsync();

            if (response != null && response.IsSuccess)
            {
                if (response.Result != null)
                {
                    listProduct = JsonSerializer.Deserialize<List<ProductDto>>(Convert.ToString(response.Result), _jsonSerializerOptions);
                }
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(listProduct);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ProductDetail(int productId)
        {
            ProductDetailDto? product = null;

            ResponseDto? response = await _productService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                if (response.Result != null)
                {
                    product = JsonSerializer.Deserialize<ProductDetailDto>(Convert.ToString(response.Result), _jsonSerializerOptions);
                }
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(product);
        }

        [HttpPost()]
        [Authorize]
        [ActionName("ProductDetail")]
        public async Task<IActionResult> ProductDetail(ProductDetailDto productDto)
        {
            CartDto cart = new CartDto
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.UniqueName)!.Value
                },
                CartDetails = new List<CartDetailsDto>
                {
                    new CartDetailsDto
                    {
                        Quantity = productDto.Quantity,
                        ProductId = productDto.ProductId,
                    }
                }
            };

            ResponseDto? response = await _cartService.UpsertShoppingCartAsync(cart);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Item was added to the Shopping Cart";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(productDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}