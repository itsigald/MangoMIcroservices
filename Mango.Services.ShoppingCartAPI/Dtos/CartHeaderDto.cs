using Mango.Services.ShoppingCartAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartAPI.Dtos
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string? CouponCode { get; set; }

        public decimal Discount { get; set; }

        public decimal CartTotal { get; set; }

        public DateTime CartInsert { get; set; }
    }
}
