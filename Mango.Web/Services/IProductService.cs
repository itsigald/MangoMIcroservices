using Mango.Web.Models;

namespace Mango.Web.Services
{
    public interface IProductService
    {
        Task<ResponseDto?> GetAllProductsAsync();
        Task<ResponseDto?> GetProductByIdAsync(int productId);
        Task<ResponseDto?> CreateProductAsync(ProductCreateDto product);
        Task<ResponseDto?> UpdateProductAsync(ProductDto product);
        Task<ResponseDto?> DeleteProductAsync(int productId);
    }
}
