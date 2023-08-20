using Mango.Services.ShoppingCartAPI.Dtos;

namespace Mango.Services.ShoppingCartAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductDto> GetProduct(int productId);
    }
}
