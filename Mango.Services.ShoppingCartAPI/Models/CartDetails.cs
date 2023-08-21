using Mango.Services.ShoppingCartAPI.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartAPI.Models
{
    public class CartDetails
    {
        [Key]
        public int CartDetailsId { get; set; }

        public int CartHeaderId { get; set; }

        [ForeignKey("CartHeaderId")]
        public CartHeader? CartHeader { get; set; } = null;

        public int ProductId { get; set; }

        [NotMapped]
        public ProductDto? Product { get; set; }

        public int Quantity { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
