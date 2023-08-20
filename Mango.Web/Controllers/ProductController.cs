using Mango.Web.Models;
using Mango.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ProductController(IProductService productService)
        {
            _productService = productService;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        [HttpGet]
        public async Task<IActionResult> ProductIndex()
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
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ResponseDto? responseDto = await _productService.GetProductByIdAsync(productId);

            if (responseDto != null && responseDto.IsSuccess)
            {
                ProductDto product = JsonSerializer.Deserialize<ProductDto>(Convert.ToString(responseDto.Result), _jsonSerializerOptions);
                return View(product);
            }
            else
            {
                TempData["error"] = responseDto?.Message;
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductCreateDto product)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? responseDto = await _productService.CreateProductAsync(product);

                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = $"Product create with successfully.";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = responseDto?.Message;
                }
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> ProductUpdate(int productId)
        {
            ResponseDto? responseDto = await _productService.GetProductByIdAsync(productId);

            if (responseDto != null && responseDto.IsSuccess)
            {
                ProductDto product = JsonSerializer.Deserialize<ProductDto>(Convert.ToString(responseDto.Result), _jsonSerializerOptions);
                return View(product);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductUpdate(ProductDto product)
        {
            ResponseDto? responseDto = await _productService.UpdateProductAsync(product);

            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = $"Coupon update with successfully.";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = responseDto?.Message;
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDelete(int productId)
        {
            ResponseDto? responseDto = await _productService.GetProductByIdAsync(productId);

            if (responseDto != null && responseDto.IsSuccess)
            {
                ProductDto product = JsonSerializer.Deserialize<ProductDto>(Convert.ToString(responseDto.Result), _jsonSerializerOptions);
                return View(product);
            }
            else
            {
                TempData["error"] = responseDto?.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto product)
        {
            ResponseDto? responseDto = await _productService.DeleteProductAsync(product.ProductId);

            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = $"Coupon delete with successfully.";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = responseDto?.Message;
            }

            return View(product);
        }
    }

}
