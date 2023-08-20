using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class ProductDetailDto : ProductDto
    {
        [Range(1, 100)]
        public int Quantity { get; set; } = 1;
    }
}
