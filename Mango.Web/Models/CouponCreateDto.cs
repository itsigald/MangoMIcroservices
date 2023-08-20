using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class CouponCreateDto
    {
        [Required]
        public string? CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
