using Mango.Services.ShoppingCartAPI.Dtos;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<ProductDto> GetProduct(int productId)
        {
            using (var client = _httpClientFactory.CreateClient("Product"))
            {
                var response = await client.GetAsync($"/api/ProductAPI/{productId}");

                if (response.IsSuccessStatusCode)
                {
                    var contextString = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(contextString))
                    {
                        ResponseDto responseDto = JsonSerializer.Deserialize<ResponseDto>(contextString, _jsonSerializerOptions);

                        if (responseDto!.IsSuccess)
                        {
                            return JsonSerializer.Deserialize<ProductDto>(Convert.ToString(responseDto.Result), _jsonSerializerOptions);
                        }
                        else
                        {
                            return new ProductDto();
                        }
                    }
                    else
                    {
                        return new ProductDto();
                    }
                }
                else
                {
                    return new ProductDto();
                }
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            using(var client = _httpClientFactory.CreateClient("Product"))
            {
                var response = await client.GetAsync("/api/ProductAPI");

                if(response.IsSuccessStatusCode)
                {
                    var contextString = await response.Content.ReadAsStringAsync();

                    if(!string.IsNullOrEmpty(contextString))
                    {
                        ResponseDto responseDto = JsonSerializer.Deserialize<ResponseDto>(contextString, _jsonSerializerOptions);
                        
                        if(responseDto!.IsSuccess)
                        {
                            return JsonSerializer.Deserialize<IEnumerable<ProductDto>>(Convert.ToString(responseDto.Result), _jsonSerializerOptions);
                        }
                        else
                        {
                            return new List<ProductDto>();
                        }
                    }
                    else
                    {
                        return new List<ProductDto>();
                    }
                }
                else
                {
                    return new List<ProductDto>();
                }
            }
        }
    }
}
