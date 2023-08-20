using Mango.Services.ShoppingCartAPI.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartAPI.Models
{
    public class CartHeader
    {
        [Key]
        public int CartHeaderId { get; set; }

        [Required]
        [MaxLength(36)]
        public string UserId { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? CouponCode { get; set; }

        [NotMapped]
        public decimal Discount { get; set; }

        [NotMapped]
        public decimal CartTotal { get; set; }

        public DateTime CartInsert { get; set; } = DateTime.Now;

        public bool Deleted { get; set; } = false;
    }
}
