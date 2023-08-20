using System.ComponentModel.DataAnnotations;

namespace Mango.Services.CouponAPI.Dtos
{
    public class CouponCreateDto
    {
        [Required]
        public string? CouponCode { get; set; }

        [Required]
        public double DiscountAmount { get; set; }

        public int MinAmount { get; set; }
    }
}
