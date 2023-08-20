using Mango.Web.Interfaces;
using Mango.Web.Models;
using System.Runtime;

namespace Mango.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _service;
        private readonly IAppSettings _settings;

        public ProductService(IBaseService service, IAppSettings settings)
        {
            _service = service;
            _settings = settings;
        }

        public async Task<ResponseDto?> GetAllProductsAsync()
        {
            var getAll = await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.GET,
                Url = $"{_settings.ProductUrlBase}{_settings.ProductAPI}"
            });

            return getAll;
        }

        public async Task<ResponseDto?> GetProductByIdAsync(int productId)
        {
            return await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.GET,
                Url = $"{_settings.ProductUrlBase}{_settings.ProductAPI}/ProductById/{productId}"
            });
        }

        public async Task<ResponseDto?> CreateProductAsync(ProductCreateDto product)
        {
            return await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.POST,
                Data = product,
                Url = $"{_settings.ProductUrlBase}{_settings.ProductAPI}"
            });
        }
        public async Task<ResponseDto?> UpdateProductAsync(ProductDto product)
        {
            return await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.PUT,
                Data = product,
                Url = $"{_settings.ProductUrlBase}{_settings.ProductAPI}"
            });
        }

        public async Task<ResponseDto?> DeleteProductAsync(int productId)
        {
            return await _service.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.DELETE,
                Url = $"{_settings.ProductUrlBase}{_settings.ProductAPI}/{productId}"
            });
        }
    }
}
